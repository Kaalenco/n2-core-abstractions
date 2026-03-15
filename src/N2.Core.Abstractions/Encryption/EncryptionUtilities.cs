// This file contains utility methods for handling encryption keys, such as generating random
// keys and creating and validating time based access tokens (TATs).
using System.Security.Cryptography;

using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.Encryption;

public static class EncryptionUtilities
{
    /// <summary>
    /// Generates a random encryption key of the specified length.
    /// </summary>
    /// <param name="length">The length of the key in bytes.</param>
    /// <returns>A random encryption key as a byte array.</returns>
    public static byte[] GenerateRandomKey(int length)
    {
    #if NET8_0_OR_GREATER
        var key = new byte[length];
        RandomNumberGenerator.Fill(key);
        return key;
    #else
        using (var rng = new RNGCryptoServiceProvider())
        {
            var key = new byte[length];
            rng.GetBytes(key);
            return key;
        }
    #endif
    }


    /// <summary>
    /// Encodes <paramref name="value"/> as UTF-8, wraps it in an <see cref="AuthenticatedValue"/>
    /// with the given expiration, and returns the encrypted, integrity-protected token string.
    /// </summary>
    /// <param name="value">The plaintext string to protect. Must not be null.</param>
    /// <param name="expiration">Expiration time for the token.</param>
    /// <param name="systemSecret">
    /// Application-wide secret used for encryption and MAC. Must be at least 32 bytes.
    /// </param>
    /// <returns>A URL-safe base64 token string suitable for storage or transmission.</returns>
    public static string ToAuthenticatedValue(this string value, DateTime expiration, byte[] systemSecret)
    {
        return (value == null) 
            ? throw new ArgumentNullException(nameof(value))
            : new AuthenticatedValue(System.Text.Encoding.UTF8.GetBytes(value), expiration)
              .ToTokenString(systemSecret);
    }

    /// <summary>
    /// Verifies and decrypts an authenticated token string produced by
    /// <see cref="ToAuthenticatedValue"/>, then returns the original string decoded as UTF-8.
    /// </summary>
    /// <param name="tokenString">The encrypted token string to verify and decrypt.</param>
    /// <param name="systemSecret">
    /// The same secret used when the token was created. Must be at least 32 bytes.
    /// </param>
    /// <param name="time">Used to verify the token has not expired.</param>
    /// <returns>The original plaintext string.</returns>
    /// <exception cref="System.Security.Cryptography.CryptographicException">
    /// Thrown when the token has been tampered with, the wrong secret is supplied, or the token has expired.
    /// </exception>
    public static string FromAuthenticatedValue(this string tokenString, byte[] systemSecret, ITimeSystem time)
    {
        var av = AuthenticatedValue.FromTokenString(tokenString, systemSecret, time);
        return av.Value.Length == 0
            ? string.Empty
            : System.Text.Encoding.UTF8.GetString(av.Value);
    }

}
