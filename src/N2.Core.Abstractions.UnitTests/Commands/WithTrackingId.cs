using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

[TestClass]
public class WithTrackingId
{
    [TestMethod]
    public void NewCreatesNonEmptyGuid()
    {
        TrackingId id = TrackingId.New();
        Assert.AreNotEqual(Guid.Empty, id.Value);
    }

    [TestMethod]
    public void ConstructorFromGuidStoresValue()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = new(guid);
        Assert.AreEqual(guid, id.Value);
    }

    [TestMethod]
    public void ConstructorFromValidStringParsesGuid()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = new(guid.ToString());
        Assert.AreEqual(guid, id.Value);
    }

    [TestMethod]
    public void ConstructorFromEmptyStringThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new TrackingId(string.Empty));
    }

    [TestMethod]
    public void ConstructorFromInvalidStringThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new TrackingId("not-a-guid"));
    }

    [TestMethod]
    public void ConstructorFromWhitespaceStringThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new TrackingId("   "));
    }

    [TestMethod]
    public void ImplicitConversionFromGuid()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = guid;
        Assert.AreEqual(guid, id.Value);
    }

    [TestMethod]
    public void ImplicitConversionToGuid()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = new(guid);
        Guid result = id;
        Assert.AreEqual(guid, result);
    }

    [TestMethod]
    public void EqualityReturnsTrueForSameValue()
    {
        Guid guid = Guid.NewGuid();
        TrackingId a = new(guid);
        TrackingId b = new(guid);
        Assert.IsTrue(a == b);
        Assert.IsFalse(a != b);
        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals((object)b));
    }

    [TestMethod]
    public void EqualityReturnsFalseForDifferentValues()
    {
        TrackingId a = new(Guid.NewGuid());
        TrackingId b = new(Guid.NewGuid());
        Assert.IsFalse(a == b);
        Assert.IsTrue(a != b);
    }

    [TestMethod]
    public void EqualsObjectReturnsFalseForNull()
    {
        TrackingId id = new(Guid.NewGuid());
        Assert.IsFalse(id.Equals(null));
    }

    [TestMethod]
    public void ToGuidReturnsUnderlyingValue()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = new(guid);
        Assert.AreEqual(guid, id.ToGuid());
    }

    [TestMethod]
    public void ToTrackingIdCreatesFromGuid()
    {
        Guid guid = Guid.NewGuid();
        TrackingId id = TrackingId.ToTrackingId(guid);
        Assert.AreEqual(guid, id.Value);
    }

    [TestMethod]
    public void GetHashCodeMatchesForEqualValues()
    {
        Guid guid = Guid.NewGuid();
        TrackingId a = new(guid);
        TrackingId b = new(guid);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
}
