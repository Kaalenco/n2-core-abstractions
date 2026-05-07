using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.Encryption;

[TestClass]
public sealed class WithAuthenticatedValue
{
    private sealed class FixedTimeSystem : ITimeSystem
    {
        private readonly DateTime _utcNow;
        public FixedTimeSystem(DateTime utcNow) => _utcNow = utcNow;
        public DateTime UtcNow => _utcNow;
        public DateTimeOffset LocalNow => new DateTimeOffset(_utcNow);
        public DateTime LocalToday => _utcNow.Date;
    }

    private static readonly byte[] TestSecret = EncryptionUtilities.GenerateRandomKey(32);
    private static readonly ITimeSystem CurrentTime = new FixedTimeSystem(DateTime.UtcNow.AddMinutes(1));

    // -------------------------------------------------------------------------
    // FromTokenString — ArgumentException guards
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_NullTokenString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            AuthenticatedValue.FromTokenString(null!, TestSecret, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_EmptyTokenString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            AuthenticatedValue.FromTokenString(string.Empty, TestSecret, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_NullTime_ThrowsArgumentException()
    {
        string token = CreateValidToken("test", DateTime.UtcNow.AddHours(1));
        Assert.Throws<ArgumentException>(() =>
            AuthenticatedValue.FromTokenString(token, TestSecret, null!));
    }

    [TestMethod]
    public void FromTokenString_NullSecret_ThrowsArgumentException()
    {
        string token = CreateValidToken("test", DateTime.UtcNow.AddHours(1));
        Assert.Throws<ArgumentException>(() =>
            AuthenticatedValue.FromTokenString(token, null!, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_SecretTooShort_ThrowsArgumentException()
    {
        string token = CreateValidToken("test", DateTime.UtcNow.AddHours(1));
        byte[] shortSecret = new byte[16];
        Assert.Throws<ArgumentException>(() =>
            AuthenticatedValue.FromTokenString(token, shortSecret, CurrentTime));
    }

    // -------------------------------------------------------------------------
    // FromTokenString — FormatException guards
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_InvalidBase64Characters_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() =>
            AuthenticatedValue.FromTokenString("not!valid!base64!!!", TestSecret, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_TokenTooShortAfterDecode_ThrowsFormatException()
    {
        // Must decode to < 64 bytes (IV=16 + min AES block=16 + MAC=32).
        byte[] tooShort = new byte[10];
        string token = Convert.ToBase64String(tooShort).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        Assert.Throws<FormatException>(() =>
            AuthenticatedValue.FromTokenString(token, TestSecret, CurrentTime));
    }

    // -------------------------------------------------------------------------
    // FromTokenString — CryptographicException guards
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_TamperedToken_ThrowsCryptographicException()
    {
        string token = CreateValidToken("hello", DateTime.UtcNow.AddHours(1));
        string tampered = FlipMiddleByte(token);
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(tampered, TestSecret, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_WrongSecret_ThrowsCryptographicException()
    {
        string token = CreateValidToken("hello", DateTime.UtcNow.AddHours(1));
        byte[] wrongSecret = EncryptionUtilities.GenerateRandomKey(32);
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(token, wrongSecret, CurrentTime));
    }

    [TestMethod]
    public void FromTokenString_ExpiredToken_ThrowsCryptographicException()
    {
        string token = CreateValidToken("payload", DateTime.UtcNow.AddDays(-1));
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(token, TestSecret, CurrentTime));
    }

    // -------------------------------------------------------------------------
    // FromTokenString — happy path variants
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_EmptyValue_RoundTripsCorrectly()
    {
        AuthenticatedValue original = new([], DateTime.UtcNow.AddHours(1));
        string token = original.ToTokenString(TestSecret);
        AuthenticatedValue result = AuthenticatedValue.FromTokenString(token, TestSecret, CurrentTime);
        Assert.AreEqual(0, result.Value.Length);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static string CreateValidToken(string value, DateTime expiry)
    {
        return new AuthenticatedValue(System.Text.Encoding.UTF8.GetBytes(value), expiry)
            .ToTokenString(TestSecret);
    }

    private static string FlipMiddleByte(string token)
    {
        string base64 = token.Replace('-', '+').Replace('_', '/');
        base64 += new string('=', (4 - base64.Length % 4) % 4);
        byte[] bytes = Convert.FromBase64String(base64);
        bytes[bytes.Length / 2] ^= 0xFF;
        return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}
