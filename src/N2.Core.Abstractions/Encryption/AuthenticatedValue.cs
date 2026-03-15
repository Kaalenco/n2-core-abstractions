// This file contains utility methods for handling encryption keys, such as generating random
// keys and creating and validating time based access tokens (TATs).
using System.Security.Cryptography; 
using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.Encryption;

/// <summary>
/// An authenticated, encrypted value with a mandatory expiration.
/// Use <see cref="ToTokenString"/> to produce a confidential, integrity-protected string
/// and <see cref="FromTokenString"/> to verify and recover the value.
/// </summary>
/// <remarks>
/// Wire format (base64url-encoded):
///   [16 bytes IV] [AES-256-CBC ciphertext of (value_length || value || UTC_ticks)] [32 bytes HMAC-SHA256(IV || ciphertext)]
/// Two keys are derived from <c>systemSecret</c> via domain-separated HMAC so the same
/// key material is never used for both encryption and authentication.
/// </remarks>
public sealed class AuthenticatedValue : IEquatable<AuthenticatedValue>
{
    /// <summary>The raw value bytes. Never null; an empty array represents an absent value.</summary>
    public byte[] Value { get; }

    /// <summary>Expiration in UTC. Always <see cref="DateTimeKind.Utc"/>.</summary>
    public DateTime Expiration { get; }

    /// <param name="value">The value bytes. Null is treated as empty.</param>
    /// <param name="expiration">Expiration time. Converted to UTC if not already.</param>
    public AuthenticatedValue(byte[] value, DateTime expiration)
    {
        Value      = value ?? [];
        Expiration = expiration.ToUniversalTime();
    }

    private const int IvLength  = 16;
    private const int MacLength = 32;
    private const string EncLabel = "N2.AuthenticatedValue.v1.enc";
    private const string MacLabel = "N2.AuthenticatedValue.v1.mac";

    /// <summary>
    /// Encrypts and serialises this value to a URL-safe base64 string.
    /// A fresh random IV is generated on every call, so the same value produces
    /// different strings across calls — this is expected and correct.
    /// </summary>
    /// <param name="systemSecret">
    /// Application-wide secret from which encryption and MAC keys are derived.
    /// Must be at least 32 bytes. Keep this secret; exposure breaks confidentiality.
    /// </param>
    public string ToTokenString(byte[] systemSecret)
    {
        ValidateSecret(systemSecret);
        var (encKey, macKey) = DeriveKeys(systemSecret);

        // Plaintext: [4 bytes: value length][value bytes][8 bytes: UTC ticks]
        // Value is guaranteed non-null and Expiration is already UTC (enforced by constructor).
        var plaintext = new byte[4 + Value.Length + 8];
        Buffer.BlockCopy(BitConverter.GetBytes(Value.Length), 0, plaintext, 0,              4);
        Buffer.BlockCopy(Value,                               0, plaintext, 4,              Value.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(Expiration.Ticks), 0, plaintext, 4 + Value.Length, 8);

        // Fresh random IV per serialisation
        var iv = new byte[IvLength];
#if NET8_0_OR_GREATER
        RandomNumberGenerator.Fill(iv);
#else
        using (var rng = new RNGCryptoServiceProvider())
            rng.GetBytes(iv);
#endif

        // AES-256-CBC encryption
        // CA5401: IV is CSPRNG-generated above (RandomNumberGenerator.Fill / RNGCryptoServiceProvider),
        // so it is non-repeatable by construction. The analyser cannot prove this statically.
#pragma warning disable CA5401
        byte[] ciphertext;
        using (var aes = Aes.Create())
        {
            aes.Key     = encKey;
            aes.IV      = iv;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var encryptor = aes.CreateEncryptor();
            ciphertext = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
        }
#pragma warning restore CA5401

        // Encrypt-then-MAC: HMAC covers IV + ciphertext
        var macInput = new byte[IvLength + ciphertext.Length];
        Buffer.BlockCopy(iv,         0, macInput, 0,        IvLength);
        Buffer.BlockCopy(ciphertext, 0, macInput, IvLength, ciphertext.Length);

        byte[] mac;
        using (var hmac = new HMACSHA256(macKey))
            mac = hmac.ComputeHash(macInput);

        // Output: IV || ciphertext || MAC
        var output = new byte[IvLength + ciphertext.Length + MacLength];
        Buffer.BlockCopy(iv,         0, output, 0,                         IvLength);
        Buffer.BlockCopy(ciphertext, 0, output, IvLength,                  ciphertext.Length);
        Buffer.BlockCopy(mac,        0, output, IvLength + ciphertext.Length, MacLength);

        return Convert.ToBase64String(output)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    /// <summary>
    /// Verifies the MAC, decrypts, and deserialises a string produced by <see cref="ToTokenString"/>.
    /// MAC verification happens before decryption to prevent padding oracle attacks.
    /// </summary>
    /// <param name="tokenString">The URL-safe base64 token string.</param>
    /// <param name="systemSecret">The same secret used during <see cref="ToTokenString"/>.</param>
    /// <param name="time">The time is used to verify the token expiration.</param>
    /// <exception cref="ArgumentException">Thrown when inputs are null/invalid.</exception>
    /// <exception cref="FormatException">Thrown when the token string is malformed.</exception>
    /// <exception cref="CryptographicException">Thrown when MAC or decryption fails.</exception>
    public static AuthenticatedValue FromTokenString(string tokenString, byte[] systemSecret, ITimeSystem time)
    {
        if (string.IsNullOrEmpty(tokenString))
            throw new ArgumentException("tokenString must not be null or empty.", nameof(tokenString));
        if (time is null)
            throw new ArgumentException("Time must not be null.", nameof(time));
        ValidateSecret(systemSecret);

        // Base64url decode
        var base64 = tokenString.Replace('-', '+').Replace('_', '/');
        base64 += new string('=', (4 - base64.Length % 4) % 4);

        byte[] encoded;
        try { encoded = Convert.FromBase64String(base64); }
        catch (FormatException ex) { throw new FormatException("Invalid token string: base64 decode failed.", ex); }

        // Minimum: IV + one AES block (16 bytes minimum ciphertext) + MAC
        if (encoded.Length < IvLength + 16 + MacLength)
            throw new FormatException("Invalid token string: too short.");

        var (encKey, macKey) = DeriveKeys(systemSecret);

        // Split: IV | ciphertext | MAC
        int ciphertextLength = encoded.Length - IvLength - MacLength;
        var iv         = new byte[IvLength];
        var ciphertext = new byte[ciphertextLength];
        var embeddedMac = new byte[MacLength];
        Buffer.BlockCopy(encoded, 0,                         iv,          0, IvLength);
        Buffer.BlockCopy(encoded, IvLength,                  ciphertext,  0, ciphertextLength);
        Buffer.BlockCopy(encoded, IvLength + ciphertextLength, embeddedMac, 0, MacLength);

        // Verify MAC before decrypting (Encrypt-then-MAC prevents padding oracle)
        var macInput = new byte[IvLength + ciphertextLength];
        Buffer.BlockCopy(iv,         0, macInput, 0,        IvLength);
        Buffer.BlockCopy(ciphertext, 0, macInput, IvLength, ciphertextLength);

        byte[] expectedMac;
        using (var hmac = new HMACSHA256(macKey))
            expectedMac = hmac.ComputeHash(macInput);

        if (!ConstantTimeEquals(embeddedMac, expectedMac))
            throw new CryptographicException("Token integrity check failed: MAC mismatch.");

        // Decrypt — safe to proceed after MAC verification
        // CA5401: IV is extracted from the authenticated token string and has already passed
        // MAC verification above; it is the same random IV generated during ToTokenString.
#pragma warning disable CA5401
        byte[] plaintext;
        try
        {
            using var aes = Aes.Create();
            aes.Key     = encKey;
            aes.IV      = iv;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var decryptor = aes.CreateDecryptor();
            plaintext = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Token decryption failed.", ex);
        }
#pragma warning restore CA5401

        // Parse plaintext: [4 bytes value length][value bytes][8 bytes ticks]
        if (plaintext.Length < 4 + 8)
            throw new FormatException("Invalid token string: plaintext too short.");

        var valueLength = BitConverter.ToInt32(plaintext, 0);
        if (valueLength < 0 || plaintext.Length != 4 + valueLength + 8)
            throw new FormatException("Invalid token string: plaintext length mismatch.");

        var value = new byte[valueLength];
        if (valueLength > 0)
            Buffer.BlockCopy(plaintext, 4, value, 0, valueLength);

        var ticks      = BitConverter.ToInt64(plaintext, 4 + valueLength);
        var expiration = new DateTime(ticks, DateTimeKind.Utc);

        if (expiration < time.UtcNow)
            throw new CryptographicException("Token has expired.");

        return new AuthenticatedValue(value, expiration);
    }

    private static void ValidateSecret(byte[] systemSecret)
    {
        if (systemSecret == null || systemSecret.Length < 32)
            throw new ArgumentException("systemSecret must be at least 32 bytes.", nameof(systemSecret));
    }

    /// <summary>
    /// Derives separate encryption and MAC keys from <paramref name="systemSecret"/>
    /// using domain-separated HMAC-SHA256 so the same key material is never reused.
    /// </summary>
    private static (byte[] encKey, byte[] macKey) DeriveKeys(byte[] systemSecret)
    {
        using var hmac = new HMACSHA256(systemSecret);
        var encKey = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(EncLabel));
        var macKey = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(MacLabel));
        return (encKey, macKey);
    }

    /// <summary>
    /// Constant-time byte array comparison — prevents timing side-channel attacks.
    /// </summary>
    private static bool ConstantTimeEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++)
            diff |= a[i] ^ b[i];
        return diff == 0;
    }

    public override bool Equals(object? obj) => obj is AuthenticatedValue other && Equals(other);

    public bool Equals(AuthenticatedValue? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Expiration == other.Expiration && Enumerable.SequenceEqual(Value, other.Value);
    }

    public override int GetHashCode()
    {
        int valueHash = 0;
        foreach (byte b in Value)
            valueHash = (valueHash * 31) + b;

#if NET5_0_OR_GREATER
        return HashCode.Combine(valueHash, Expiration);
#else
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + valueHash;
            hash = hash * 23 + Expiration.GetHashCode();
            return hash;
        }
#endif
    }

    public static bool operator ==(AuthenticatedValue? left, AuthenticatedValue? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(AuthenticatedValue? left, AuthenticatedValue? right)
        => !(left == right);
}