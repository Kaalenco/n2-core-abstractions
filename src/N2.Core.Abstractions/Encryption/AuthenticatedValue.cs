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
        // Unspecified kind is treated as UTC (not local time) so expiry does not depend on server time zone.
        Expiration = expiration.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(expiration, DateTimeKind.Utc)
            : expiration.ToUniversalTime();
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
    public string ToTokenString(byte[] systemSecret) => ToTokenString(systemSecret, string.Empty);

    /// <summary>
    /// Like <see cref="ToTokenString(byte[])"/> but binds the token to a <paramref name="purpose"/>
    /// (for example <c>"email-confirmation"</c>). A token created for one purpose fails
    /// verification for any other purpose, which prevents cross-feature token reuse.
    /// An empty purpose is the legacy v1 behaviour.
    /// </summary>
    public string ToTokenString(byte[] systemSecret, string purpose)
    {
        ValidateSecret(systemSecret);
        var (encKey, macKey) = DeriveKeys(systemSecret, purpose);
        try
        {
            return Serialise(encKey, macKey);
        }
        finally
        {
            Zero(encKey);
            Zero(macKey);
        }
    }

    private string Serialise(byte[] encKey, byte[] macKey)
    {

        // Plaintext: [4 bytes: value length][value bytes][8 bytes: UTC ticks]
        // Value is guaranteed non-null and Expiration is already UTC (enforced by constructor).
        var plaintext = new byte[4 + Value.Length + 8];
        WriteLittleEndian(plaintext, 0, (uint)Value.Length, 4);
        Buffer.BlockCopy(Value, 0, plaintext, 4, Value.Length);
        WriteLittleEndian(plaintext, 4 + Value.Length, (ulong)Expiration.Ticks, 8);

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
        Zero(plaintext);
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
        => FromTokenString(tokenString, systemSecret, time, string.Empty);

    /// <summary>
    /// Non-throwing variant for request paths: returns false for every invalid, tampered,
    /// wrong-purpose or expired token without revealing why. Invalid arguments
    /// (null secret/time) still throw, as they are programming errors.
    /// </summary>
    public static bool TryFromTokenString(string? tokenString, byte[] systemSecret, ITimeSystem time,
        string purpose, out AuthenticatedValue? value)
    {
        value = null;
        if (string.IsNullOrEmpty(tokenString)) return false;
        try
        {
            value = FromTokenString(tokenString!, systemSecret, time, purpose);
            return true;
        }
        catch (Exception ex) when (ex is FormatException || ex is CryptographicException)
        {
            return false;
        }
    }

    /// <summary>
    /// Verifies a token created with <see cref="ToTokenString(byte[], string)"/> for the same
    /// <paramref name="purpose"/>. An empty purpose is the legacy v1 behaviour.
    /// </summary>
    public static AuthenticatedValue FromTokenString(string tokenString, byte[] systemSecret, ITimeSystem time, string purpose)
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

        var (encKey, macKey) = DeriveKeys(systemSecret, purpose);
        try
        {
            return Deserialise(encoded, encKey, macKey, time);
        }
        finally
        {
            Zero(encKey);
            Zero(macKey);
        }
    }

    private static AuthenticatedValue Deserialise(byte[] encoded, byte[] encKey, byte[] macKey, ITimeSystem time)
    {
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

        var valueLength = unchecked((int)ReadLittleEndian(plaintext, 0, 4));
        if (valueLength < 0 || plaintext.Length != 4 + valueLength + 8)
            throw new FormatException("Invalid token string: plaintext length mismatch.");

        var value = new byte[valueLength];
        if (valueLength > 0)
            Buffer.BlockCopy(plaintext, 4, value, 0, valueLength);

        var ticks = unchecked((long)ReadLittleEndian(plaintext, 4 + valueLength, 8));
        if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
            throw new FormatException("Invalid token string: expiration out of range.");
        var expiration = new DateTime(ticks, DateTimeKind.Utc);
        Zero(plaintext);

        if (expiration < time.UtcNow)
            throw new CryptographicException("Token has expired.");

        return new AuthenticatedValue(value, expiration);
    }

    private static void ValidateSecret(byte[] systemSecret)
    {
        if (systemSecret == null || systemSecret.Length < 32)
            throw new ArgumentException("systemSecret must be at least 32 bytes.", nameof(systemSecret));

        // Reject trivially weak secrets (e.g. all zero, or a short pattern repeated).
        var distinct = new HashSet<byte>(systemSecret);
        if (distinct.Count < 8)
            throw new ArgumentException(
                "systemSecret is too weak: it must be random (use EncryptionUtilities.GenerateRandomKey(32)).",
                nameof(systemSecret));
    }

    private static void Zero(byte[] buffer)
    {
#if NETSTANDARD2_0
        Array.Clear(buffer, 0, buffer.Length);
#else
        CryptographicOperations.ZeroMemory(buffer);
#endif
    }

    private static void WriteLittleEndian(byte[] buffer, int offset, ulong value, int count)
    {
        for (int i = 0; i < count; i++)
            buffer[offset + i] = (byte)(value >> (8 * i));
    }

    private static ulong ReadLittleEndian(byte[] buffer, int offset, int count)
    {
        ulong result = 0;
        for (int i = 0; i < count; i++)
            result |= (ulong)buffer[offset + i] << (8 * i);
        return result;
    }

    /// <summary>
    /// Derives separate encryption and MAC keys from <paramref name="systemSecret"/>
    /// using domain-separated HMAC-SHA256 so the same key material is never reused.
    /// </summary>
    private static (byte[] encKey, byte[] macKey) DeriveKeys(byte[] systemSecret, string? purpose)
    {
        // An empty purpose keeps the exact v1 labels so existing tokens remain valid.
        var suffix = string.IsNullOrEmpty(purpose) ? string.Empty : "|" + purpose;
        using var hmac = new HMACSHA256(systemSecret);
        var encKey = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(EncLabel + suffix));
        var macKey = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(MacLabel + suffix));
        return (encKey, macKey);
    }

    /// <summary>
    /// Constant-time byte array comparison — prevents timing side-channel attacks.
    /// </summary>
    private static bool ConstantTimeEquals(byte[] a, byte[] b)
    {
#if !NETSTANDARD2_0
        return CryptographicOperations.FixedTimeEquals(a, b);
#else
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++)
            diff |= a[i] ^ b[i];
        return diff == 0;
#endif
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