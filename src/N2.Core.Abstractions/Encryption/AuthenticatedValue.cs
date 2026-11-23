// This file contains utility methods for handling encryption keys, such as generating random
// keys and creating and validating time based access tokens (TATs).
using System.Security.Cryptography;
using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.Encryption;

/// <summary>
/// An authenticated, encrypted value with a mandatory expiration, bound to a purpose.
/// Use <see cref="ToTokenString"/> to produce a confidential, integrity-protected string
/// and <see cref="FromTokenString(string, byte[], ITimeSystem, string)"/> to verify and recover the value.
/// </summary>
/// <remarks>
/// Wire format v2 (base64url-encoded):
///   [1 byte version = 2] [1 byte key id] [16 bytes IV]
///   [AES-256-CBC ciphertext of (value_length(LE) || value || UTC_ticks(LE))]
///   [32 bytes HMAC-SHA256(everything before the MAC)]
/// Encryption and MAC keys are derived with HKDF-SHA256 (RFC 5869) from the secret, using the
/// info strings <c>N2.AuthenticatedValue.v2.enc|purpose</c> and <c>N2.AuthenticatedValue.v2.mac|purpose</c>.
/// The version, key id and purpose are all authenticated. v1 tokens are not accepted.
/// The expiry check has no clock-skew allowance; allow for skew yourself in multi-node deployments.
/// </remarks>
public sealed class AuthenticatedValue : IEquatable<AuthenticatedValue>
{
    /// <summary>The raw value bytes. Never null; an empty array represents an absent value.</summary>
    public byte[] Value { get; }

    /// <summary>Expiration in UTC. Always <see cref="DateTimeKind.Utc"/>.</summary>
    public DateTime Expiration { get; }

    /// <param name="value">The value bytes. Null is treated as empty.</param>
    /// <param name="expiration">Expiration time. Unspecified kind is treated as UTC.</param>
    public AuthenticatedValue(byte[] value, DateTime expiration)
    {
        Value      = value ?? [];
        Expiration = expiration.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(expiration, DateTimeKind.Utc)
            : expiration.ToUniversalTime();
    }

    private const byte FormatVersion = 2;
    private const int HeaderLength = 2;
    private const int IvLength  = 16;
    private const int MacLength = 32;
    private const string EncLabel = "N2.AuthenticatedValue.v2.enc|";
    private const string MacLabel = "N2.AuthenticatedValue.v2.mac|";

    /// <summary>
    /// Encrypts and serialises this value to a URL-safe base64 string. A fresh random IV is used
    /// on every call, so the same value produces different strings across calls.
    /// </summary>
    /// <param name="systemSecret">Random secret of at least 32 bytes (see <see cref="EncryptionUtilities.GenerateRandomKey"/>).</param>
    /// <param name="purpose">What the token is for, e.g. "email-confirmation". Verification fails for any other purpose.</param>
    /// <param name="keyId">Identifies the secret in the verifier's list, enabling key rotation.</param>
    public string ToTokenString(byte[] systemSecret, string purpose, byte keyId = 0)
    {
        ValidateSecret(systemSecret);
        ValidatePurpose(purpose);
        var (encKey, macKey) = DeriveKeys(systemSecret, purpose);
        var plaintext = new byte[4 + Value.Length + 8];
        try
        {
            WriteLittleEndian(plaintext, 0, (uint)Value.Length, 4);
            Buffer.BlockCopy(Value, 0, plaintext, 4, Value.Length);
            WriteLittleEndian(plaintext, 4 + Value.Length, (ulong)Expiration.Ticks, 8);

            var iv = new byte[IvLength];
#if NET8_0_OR_GREATER
            RandomNumberGenerator.Fill(iv);
#else
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(iv);
#endif

            // CA5401: IV is CSPRNG-generated above, so it is non-repeatable by construction.
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

            // Layout: header | IV | ciphertext | MAC (MAC covers everything before it)
            var output = new byte[HeaderLength + IvLength + ciphertext.Length + MacLength];
            output[0] = FormatVersion;
            output[1] = keyId;
            Buffer.BlockCopy(iv,         0, output, HeaderLength,            IvLength);
            Buffer.BlockCopy(ciphertext, 0, output, HeaderLength + IvLength, ciphertext.Length);

            var authenticatedLength = output.Length - MacLength;
            using (var hmac = new HMACSHA256(macKey))
            {
                var mac = hmac.ComputeHash(output, 0, authenticatedLength);
                Buffer.BlockCopy(mac, 0, output, authenticatedLength, MacLength);
            }

            return Convert.ToBase64String(output).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
        finally
        {
            Zero(encKey);
            Zero(macKey);
            Zero(plaintext);
        }
    }

    /// <summary>
    /// Verifies the MAC, decrypts, and deserialises a token. The MAC is verified before decryption.
    /// The token must carry key id 0.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when inputs are null/invalid.</exception>
    /// <exception cref="FormatException">Thrown when the token string is malformed.</exception>
    /// <exception cref="CryptographicException">Thrown when MAC or decryption fails, or the token has expired.</exception>
    public static AuthenticatedValue FromTokenString(string tokenString, byte[] systemSecret, ITimeSystem time, string purpose)
        => FromTokenString(tokenString, new[] { systemSecret }, time, purpose);

    /// <summary>
    /// Verifies a token against a list of secrets for key rotation. A token's key id is the index
    /// into <paramref name="systemSecrets"/>: append new secrets, never reorder or remove while
    /// tokens signed with them can still be valid.
    /// </summary>
    public static AuthenticatedValue FromTokenString(string tokenString, IReadOnlyList<byte[]> systemSecrets, ITimeSystem time, string purpose)
    {
        if (string.IsNullOrEmpty(tokenString))
            throw new ArgumentException("tokenString must not be null or empty.", nameof(tokenString));
        if (time is null)
            throw new ArgumentException("Time must not be null.", nameof(time));
        if (systemSecrets is null || systemSecrets.Count == 0)
            throw new ArgumentException("At least one secret is required.", nameof(systemSecrets));
        foreach (var secret in systemSecrets) ValidateSecret(secret);
        ValidatePurpose(purpose);

        var base64 = tokenString.Replace('-', '+').Replace('_', '/');
        base64 += new string('=', (4 - base64.Length % 4) % 4);

        byte[] encoded;
        try { encoded = Convert.FromBase64String(base64); }
        catch (FormatException ex) { throw new FormatException("Invalid token string: base64 decode failed.", ex); }

        if (encoded.Length < HeaderLength + IvLength + 16 + MacLength)
            throw new FormatException("Invalid token string: too short.");
        if (encoded[0] != FormatVersion)
            throw new FormatException("Invalid token string: unsupported version.");
        var keyId = encoded[1];
        if (keyId >= systemSecrets.Count)
            throw new CryptographicException("Token integrity check failed: unknown key.");

        var (encKey, macKey) = DeriveKeys(systemSecrets[keyId], purpose);
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

    /// <summary>
    /// Non-throwing variant for request paths: returns false for every malformed, tampered,
    /// wrong-purpose, unknown-key or expired token without revealing why. Invalid arguments
    /// (null/weak secret, null time, empty purpose) still throw, as they are programming errors.
    /// </summary>
    public static bool TryFromTokenString(string? tokenString, IReadOnlyList<byte[]> systemSecrets, ITimeSystem time,
        string purpose, out AuthenticatedValue? value)
    {
        value = null;
        if (string.IsNullOrEmpty(tokenString)) return false;
        try
        {
            value = FromTokenString(tokenString!, systemSecrets, time, purpose);
            return true;
        }
        catch (Exception ex) when (ex is FormatException || ex is CryptographicException)
        {
            return false;
        }
    }

    /// <inheritdoc cref="TryFromTokenString(string?, IReadOnlyList{byte[]}, ITimeSystem, string, out AuthenticatedValue?)"/>
    public static bool TryFromTokenString(string? tokenString, byte[] systemSecret, ITimeSystem time,
        string purpose, out AuthenticatedValue? value)
        => TryFromTokenString(tokenString, new[] { systemSecret }, time, purpose, out value);

    private static AuthenticatedValue Deserialise(byte[] encoded, byte[] encKey, byte[] macKey, ITimeSystem time)
    {
        int authenticatedLength = encoded.Length - MacLength;
        var embeddedMac = new byte[MacLength];
        Buffer.BlockCopy(encoded, authenticatedLength, embeddedMac, 0, MacLength);

        byte[] expectedMac;
        using (var hmac = new HMACSHA256(macKey))
            expectedMac = hmac.ComputeHash(encoded, 0, authenticatedLength);

        if (!ConstantTimeEquals(embeddedMac, expectedMac))
            throw new CryptographicException("Token integrity check failed: MAC mismatch.");

        var iv = new byte[IvLength];
        Buffer.BlockCopy(encoded, HeaderLength, iv, 0, IvLength);
        int ciphertextOffset = HeaderLength + IvLength;
        int ciphertextLength = authenticatedLength - ciphertextOffset;

        // CA5401: IV comes from the authenticated token and has already passed MAC verification.
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
            plaintext = decryptor.TransformFinalBlock(encoded, ciphertextOffset, ciphertextLength);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Token decryption failed.", ex);
        }
#pragma warning restore CA5401

        try
        {
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

            if (expiration < time.UtcNow)
                throw new CryptographicException("Token has expired.");

            return new AuthenticatedValue(value, expiration);
        }
        finally
        {
            Zero(plaintext);
        }
    }

    private static void ValidatePurpose(string purpose)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("purpose must not be null or empty.", nameof(purpose));
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

    /// <summary>
    /// Derives separate encryption and MAC keys with HKDF-SHA256 (RFC 5869, zero salt).
    /// Each key is one 32-byte HKDF output block, so the expand step is a single HMAC.
    /// </summary>
    private static (byte[] encKey, byte[] macKey) DeriveKeys(byte[] systemSecret, string purpose)
    {
        byte[] prk;
        using (var extract = new HMACSHA256(new byte[32]))
            prk = extract.ComputeHash(systemSecret);
        try
        {
            return (Expand(prk, EncLabel + purpose), Expand(prk, MacLabel + purpose));
        }
        finally
        {
            Zero(prk);
        }
    }

    private static byte[] Expand(byte[] prk, string info)
    {
        var infoBytes = System.Text.Encoding.UTF8.GetBytes(info);
        var input = new byte[infoBytes.Length + 1];
        Buffer.BlockCopy(infoBytes, 0, input, 0, infoBytes.Length);
        input[infoBytes.Length] = 0x01;
        using var hmac = new HMACSHA256(prk);
        return hmac.ComputeHash(input);
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

    /// <summary>Constant-time byte array comparison — prevents timing side-channel attacks.</summary>
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