using System.Diagnostics;
using System.Security.Cryptography;

using N2.Core.Abstractions.Encryption;
using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.UnitTests.Encryption;

/// <summary>
/// Provides unit tests for functionality related to authenticated values.
/// It tests edge cases (e.g., null or empty inputs) and ensures that the
/// authenticated value behaves as expected under various conditions. Round
/// trip tests (encrypting and then decrypting) are included to verify data
/// integrity. It also checks the extension methods in the EncryptionUtilities
/// class to ensure they correctly handle authenticated values.
/// </summary>
[TestClass]
public class UsingAuthenticatedValue
{
    // 32 non-zero bytes — meets the minimum secret length requirement
    private static readonly byte[] Secret = Enumerable.Range(1, 32).Select(i => (byte)i).ToArray();
    private static readonly DateTime FutureExpiry = new(2099, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime PastExpiry   = new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Returns a FakeTimeSystem set to a moment well before FutureExpiry
    private static FakeTimeSystem Now => new() { UtcNow = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc) };

    private sealed class FakeTimeSystem : ITimeSystem
    {
        public DateTime UtcNow { get; set; }
        public DateTimeOffset LocalNow => new(UtcNow);
        public DateTime LocalToday => UtcNow.Date;
    }

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Constructor_NullValue_TreatedAsEmpty()
    {
        var av = new AuthenticatedValue(null!, FutureExpiry);
        Assert.AreEqual(0, av.Value.Length);
    }

    [TestMethod]
    public void Constructor_ValueStored()
    {
        byte[] bytes = [1, 2, 3];
        var av = new AuthenticatedValue(bytes, FutureExpiry);
        CollectionAssert.AreEqual(bytes, av.Value);
    }

    [TestMethod]
    public void Constructor_NonUtcExpiration_ConvertedToUtc()
    {
        var local = new DateTime(2099, 6, 1, 12, 0, 0, DateTimeKind.Local);
        var av = new AuthenticatedValue(new byte[1], local);
        Assert.AreEqual(DateTimeKind.Utc, av.Expiration.Kind);
    }

    [TestMethod]
    public void Constructor_UtcExpiration_UnchangedKind()
    {
        var av = new AuthenticatedValue(new byte[1], FutureExpiry);
        Assert.AreEqual(DateTimeKind.Utc, av.Expiration.Kind);
        Assert.AreEqual(FutureExpiry, av.Expiration);
    }

    // -------------------------------------------------------------------------
    // ToTokenString — argument validation
    // -------------------------------------------------------------------------

    [TestMethod]
    public void ToTokenString_NullSecret_Throws()
    {
        var av = new AuthenticatedValue([1], FutureExpiry);
        Assert.Throws<ArgumentException>(() => av.ToTokenString(null!));
    }

    [TestMethod]
    public void ToTokenString_SecretTooShort_Throws()
    {
        var av = new AuthenticatedValue([1], FutureExpiry);
        Assert.Throws<ArgumentException>(() => av.ToTokenString(new byte[31]));
    }

    [TestMethod]
    public void ToTokenString_SecretExactly32Bytes_Succeeds()
    {
        var av = new AuthenticatedValue([1], FutureExpiry);
        var token = av.ToTokenString(Secret);
        Assert.IsNotNull(token);
    }

    // -------------------------------------------------------------------------
    // ToTokenString — output format
    // -------------------------------------------------------------------------

    [TestMethod]
    public void ToTokenString_OutputIsUrlSafeBase64()
    {
        var av = new AuthenticatedValue([0xFF, 0xFE, 0xFD], FutureExpiry);
        var token = av.ToTokenString(Secret);
        Assert.IsFalse(token.Contains('+',StringComparison.Ordinal), "Token must not contain '+'");
        Assert.IsFalse(token.Contains('/', StringComparison.Ordinal), "Token must not contain '/'");
        Assert.IsFalse(token.Contains('=', StringComparison.Ordinal), "Token must not contain padding '='");
    }

    [TestMethod]
    public void ToTokenString_SameValueProducesDifferentTokens()
    {
        // The random IV means every serialisation must be unique
        var av = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        var token1 = av.ToTokenString(Secret);
        var token2 = av.ToTokenString(Secret);
        Assert.AreNotEqual(token1, token2);
    }

    // -------------------------------------------------------------------------
    // FromTokenString — argument validation
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_NullToken_Throws()
    {
        Assert.Throws<ArgumentException>(() => AuthenticatedValue.FromTokenString(null!, Secret, Now));
    }

    [TestMethod]
    public void FromTokenString_EmptyToken_Throws()
    {
        Assert.Throws<ArgumentException>(() => AuthenticatedValue.FromTokenString(string.Empty, Secret, Now));
    }

    [TestMethod]
    public void FromTokenString_NullSecret_Throws()
    {
        var token = new AuthenticatedValue([1], FutureExpiry).ToTokenString(Secret);
        Assert.Throws<ArgumentException>(() => AuthenticatedValue.FromTokenString(token, null!, Now));
    }

    [TestMethod]
    public void FromTokenString_SecretTooShort_Throws()
    {
        var token = new AuthenticatedValue([1], FutureExpiry).ToTokenString(Secret);
        Assert.Throws<ArgumentException>(() => AuthenticatedValue.FromTokenString(token, new byte[31], Now));
    }

    [TestMethod]
    public void FromTokenString_NullTime_Throws()
    {
        var token = new AuthenticatedValue([1], FutureExpiry).ToTokenString(Secret);
        Assert.Throws<ArgumentException>(() => AuthenticatedValue.FromTokenString(token, Secret, null!));
    }

    // -------------------------------------------------------------------------
    // FromTokenString — security
    // -------------------------------------------------------------------------

    [TestMethod]
    public void FromTokenString_WrongSecret_Throws()
    {
        var token = new AuthenticatedValue([1, 2, 3], FutureExpiry).ToTokenString(Secret);
        var wrongSecret = Enumerable.Range(50, 32).Select(i => (byte)i).ToArray();

        Assert.Throws<CryptographicException>(() => AuthenticatedValue.FromTokenString(token, wrongSecret, Now));
    }

    [TestMethod]
    public void FromTokenString_TamperedFirstByte_Throws()
    {
        // Modifying the first character changes the IV portion, which is covered by the MAC
        var token = new AuthenticatedValue([1, 2, 3], FutureExpiry).ToTokenString(Secret);
        var chars = token.ToCharArray();
        chars[0] = chars[0] == 'A' ? 'B' : 'A';

        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(new string(chars), Secret, Now));
    }

    [TestMethod]
    public void FromTokenString_MalformedToken_Throws()
    {
        Assert.Throws<FormatException>(() =>
            AuthenticatedValue.FromTokenString("this-is-not-a-valid-token!!", Secret, Now));
    }

    [TestMethod]
    public void FromTokenString_ExpiredToken_Throws()
    {
        var token = new AuthenticatedValue([1, 2, 3], PastExpiry).ToTokenString(Secret);

        Assert.Throws<CryptographicException>(() => AuthenticatedValue.FromTokenString(token, Secret, Now));
    }

    // -------------------------------------------------------------------------
    // Expiry boundary
    // -------------------------------------------------------------------------

    [TestMethod]
    public void ExpiryBoundary_TokenAtExactExpiryMoment_IsValid()
    {
        // The check is expiration < time.UtcNow, so equality means the token is still valid.
        var expiry = new DateTime(2030, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var time   = new FakeTimeSystem { UtcNow = expiry };
        var token  = new AuthenticatedValue([1], expiry).ToTokenString(Secret);

        var result = AuthenticatedValue.FromTokenString(token, Secret, time);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ExpiryBoundary_TokenOneTickBeforeExpiry_IsValid()
    {
        var expiry = new DateTime(2030, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var time   = new FakeTimeSystem { UtcNow = expiry.AddTicks(-1) };
        var token  = new AuthenticatedValue([1], expiry).ToTokenString(Secret);

        var result = AuthenticatedValue.FromTokenString(token, Secret, time);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ExpiryBoundary_TokenOneTickPastExpiry_Throws()
    {
        var expiry = new DateTime(2030, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var time   = new FakeTimeSystem { UtcNow = expiry.AddTicks(1) };
        var token  = new AuthenticatedValue([1], expiry).ToTokenString(Secret);

        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(token, Secret, time));
    }

    // -------------------------------------------------------------------------
    // Timing-attack resistance
    // -------------------------------------------------------------------------

    [TestMethod]
    public void TimingAttack_MacMismatchAtFirstByte_ThrowsCryptographicException()
    {
        // A non-constant-time comparison might short-circuit here and return early.
        var tampered = FlipMacByte(0);
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(tampered, Secret, Now));
    }

    [TestMethod]
    public void TimingAttack_MacMismatchAtLastByte_ThrowsCryptographicException()
    {
        // A non-constant-time comparison would never reach the last byte if an earlier
        // byte already differed, yet this must still fail.
        var tampered = FlipMacByte(MacBytesLength - 1);
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(tampered, Secret, Now));
    }

    [TestMethod]
    public void TimingAttack_MacMismatchAtMiddleByte_ThrowsCryptographicException()
    {
        var tampered = FlipMacByte(MacBytesLength / 2);
        Assert.Throws<CryptographicException>(() =>
            AuthenticatedValue.FromTokenString(tampered, Secret, Now));
    }

    [TestMethod]
    public void TimingAttack_MacVerification_TimingIsConsistent()
    {
        // Verify that the time to reject a MAC mismatch at byte 0 is similar to
        // the time at byte 31.  A constant-time XOR-accumulator processes every byte
        // unconditionally; a short-circuit comparison would show a large ratio.
        const int warmup     = 200;
        const int iterations = 1_000;

        var earlyMismatch = FlipMacByte(0);
        var lateMismatch  = FlipMacByte(MacBytesLength - 1);

        // Warm up the JIT so first-run variance does not skew the measurement.
        for (int i = 0; i < warmup; i++)
        {
            TryFromToken(earlyMismatch);
            TryFromToken(lateMismatch);
        }

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            TryFromToken(earlyMismatch);
        long earlyTicks = sw.ElapsedTicks;

        sw.Restart();
        for (int i = 0; i < iterations; i++)
            TryFromToken(lateMismatch);
        long lateTicks = sw.ElapsedTicks;

        // Allow up to 4× variance to absorb GC/scheduler noise.
        // A short-circuit comparison would show orders-of-magnitude difference.
        double ratio = (double)Math.Max(earlyTicks, lateTicks) / Math.Min(earlyTicks, lateTicks);
        Assert.IsTrue(ratio < 4.0,
            $"Timing ratio {ratio:F2} suggests non-constant-time MAC comparison " +
            $"(early={earlyTicks} ticks, late={lateTicks} ticks over {iterations} iterations).");
    }

    // Token format: [IV: 16 bytes][ciphertext][MAC: 32 bytes]
    private const int MacBytesLength = 32;

    /// <summary>
    /// Produces a token string identical to a freshly-generated valid token except
    /// that one byte in the embedded MAC (at <paramref name="macOffset"/> from the
    /// start of the MAC region) is bit-flipped.
    /// </summary>
    private static string FlipMacByte(int macOffset)
    {
        var token  = new AuthenticatedValue([1, 2, 3], FutureExpiry).ToTokenString(Secret);
        var base64 = token.Replace('-', '+').Replace('_', '/');
        base64 += new string('=', (4 - base64.Length % 4) % 4);

        var bytes    = Convert.FromBase64String(base64);
        int macStart = bytes.Length - MacBytesLength;
        bytes[macStart + macOffset] ^= 0xFF;

        return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    private static void TryFromToken(string token)
    {
        try { AuthenticatedValue.FromTokenString(token, Secret, Now); }
        catch (CryptographicException) { /* expected — we are only measuring elapsed time */ }
    }

    // -------------------------------------------------------------------------
    // Round-trip — AuthenticatedValue
    // -------------------------------------------------------------------------

    [TestMethod]
    public void RoundTrip_ByteValue_Preserved()
    {
        byte[] original = [10, 20, 30, 40, 50];
        var token = new AuthenticatedValue(original, FutureExpiry).ToTokenString(Secret);
        var recovered = AuthenticatedValue.FromTokenString(token, Secret, Now);
        CollectionAssert.AreEqual(original, recovered.Value);
    }

    [TestMethod]
    public void RoundTrip_EmptyValue_Preserved()
    {
        var token = new AuthenticatedValue([], FutureExpiry).ToTokenString(Secret);
        var recovered = AuthenticatedValue.FromTokenString(token, Secret, Now);
        Assert.AreEqual(0, recovered.Value.Length);
    }

    [TestMethod]
    public void RoundTrip_Expiration_Preserved()
    {
        var expiry = new DateTime(2088, 7, 15, 9, 30, 0, DateTimeKind.Utc);
        var token = new AuthenticatedValue([42], expiry).ToTokenString(Secret);
        var recovered = AuthenticatedValue.FromTokenString(token, Secret, Now);
        Assert.AreEqual(expiry, recovered.Expiration);
        Assert.AreEqual(DateTimeKind.Utc, recovered.Expiration.Kind);
    }

    // -------------------------------------------------------------------------
    // Round-trip — extension methods
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Extension_RoundTrip_StringPreserved()
    {
        const string plaintext = "Hello, World!";
        var token = plaintext.ToAuthenticatedValue(FutureExpiry, Secret);
        var recovered = token.FromAuthenticatedValue(Secret, Now);
        Assert.AreEqual(plaintext, recovered);
    }

    [TestMethod]
    public void Extension_RoundTrip_EmptyString_Preserved()
    {
        var token = string.Empty.ToAuthenticatedValue(FutureExpiry, Secret);
        var recovered = token.FromAuthenticatedValue(Secret, Now);
        Assert.AreEqual(string.Empty, recovered);
    }

    [TestMethod]
    public void Extension_RoundTrip_UnicodePreserved()
    {
        const string plaintext = "Héllo wörld — 日本語 \U0001F389";
        var token = plaintext.ToAuthenticatedValue(FutureExpiry, Secret);
        var recovered = token.FromAuthenticatedValue(Secret, Now);
        Assert.AreEqual(plaintext, recovered);
    }

    [TestMethod]
    public void Extension_ToAuthenticatedValue_NullString_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ((string)null!).ToAuthenticatedValue(FutureExpiry, Secret));
    }

    [TestMethod]
    public void Extension_FromAuthenticatedValue_ExpiredToken_Throws()
    {
        var token = "hello".ToAuthenticatedValue(PastExpiry, Secret);
        Assert.Throws<CryptographicException>(() => token.FromAuthenticatedValue(Secret, Now));
    }

    // -------------------------------------------------------------------------
    // Equality
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Equality_SameBytesAndExpiry_AreEqual()
    {
        var a = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        var b = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        Assert.AreEqual(a, b);
        Assert.IsTrue(a == b);
        Assert.IsFalse(a != b);
    }

    [TestMethod]
    public void Equality_DifferentBytes_NotEqual()
    {
        var a = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        var b = new AuthenticatedValue([4, 5, 6], FutureExpiry);
        Assert.AreNotEqual(a, b);
        Assert.IsTrue(a != b);
    }

    [TestMethod]
    public void Equality_DifferentExpiry_NotEqual()
    {
        var a = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        var b = new AuthenticatedValue([1, 2, 3], new DateTime(2088, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void Equality_NullComparison_NotEqual()
    {
        var a = new AuthenticatedValue([1], FutureExpiry);
        // Suppress CA1508: intentionally testing operator behaviour with null
#pragma warning disable CA1508
        Assert.IsFalse(a.Equals(null!));
        Assert.IsTrue(a != null!);
        Assert.IsTrue(null! != a);
#pragma warning restore CA1508
    }

    [TestMethod]
    public void Equality_SameReference_Equal()
    {
        var a = new AuthenticatedValue([1, 2], FutureExpiry);
        // Suppress CS1718/CA1508: intentionally testing that == and Equals handle self-reference
#pragma warning disable CS1718, CA1508
        Assert.IsTrue(a.Equals(a));
        Assert.IsTrue(a == a);
#pragma warning restore CS1718, CA1508
    }

    [TestMethod]
    public void GetHashCode_EqualInstances_SameHashCode()
    {
        var a = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        var b = new AuthenticatedValue([1, 2, 3], FutureExpiry);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
}
