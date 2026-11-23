using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2.Core.SystemAbstractions;

namespace N2.Core.Abstractions.Encryption;

[TestClass]
public sealed class UsingEncryptionUtilities
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
    private const string Purpose = "test";

    [TestMethod]
    public void GenerateRandomKey_ProducesArrayOfRequestedLength()
    {
        byte[] key = EncryptionUtilities.GenerateRandomKey(32);
        Assert.AreEqual(32, key.Length);
    }

    [TestMethod]
    public void GenerateRandomKey_TwoCallsProduceDifferentValues()
    {
        byte[] key1 = EncryptionUtilities.GenerateRandomKey(32);
        byte[] key2 = EncryptionUtilities.GenerateRandomKey(32);
        CollectionAssert.AreNotEqual(key1, key2);
    }

    [TestMethod]
    public void ToAuthenticatedValue_NullValue_ThrowsArgumentNullException()
    {
        string? value = null;
        DateTime expiry = DateTime.UtcNow.AddHours(1);
        Assert.Throws<ArgumentNullException>(() => value!.ToAuthenticatedValue(expiry, TestSecret, Purpose));
    }

    [TestMethod]
    public void ToFromAuthenticatedValue_RoundTripsOriginalString()
    {
        string original = "hello world";
        DateTime expiry = DateTime.UtcNow.AddHours(1);
        ITimeSystem time = new FixedTimeSystem(DateTime.UtcNow);

        string token = original.ToAuthenticatedValue(expiry, TestSecret, Purpose);
        string result = token.FromAuthenticatedValue(TestSecret, time, Purpose);

        Assert.AreEqual(original, result);
    }

    [TestMethod]
    public void FromAuthenticatedValue_ExpiredToken_ThrowsCryptographicException()
    {
        string original = "payload";
        DateTime expiry = DateTime.UtcNow.AddSeconds(-1);
        ITimeSystem time = new FixedTimeSystem(DateTime.UtcNow);

        string token = original.ToAuthenticatedValue(expiry, TestSecret, Purpose);
        Assert.Throws<CryptographicException>(() => token.FromAuthenticatedValue(TestSecret, time, Purpose));
    }
}
