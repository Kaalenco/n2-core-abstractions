using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Entity;

[TestClass]
public sealed class WithRecord
{
    private sealed class TestRecord : Record { }

    [TestMethod]
    public void DefaultCtorSetsIdZeroAndFalseFlags()
    {
        TestRecord record = new();
        Assert.AreEqual(0, record.Id);
        Assert.IsFalse(record.IsDeleted);
        Assert.IsFalse(record.IsLocked);
    }

    [TestMethod]
    public void Uuid_IsNotEmpty()
    {
        TestRecord record = new();
        Assert.AreNotEqual(Guid.Empty, record.Uuid);
    }

    [TestMethod]
    public void Created_DefaultsToUtcKind()
    {
        TestRecord record = new();
        Assert.AreEqual(DateTimeKind.Utc, record.Created.Kind);
    }

    [TestMethod]
    public void LastModified_DefaultsToUtcKind()
    {
        TestRecord record = new();
        Assert.AreEqual(DateTimeKind.Utc, record.LastModified.Kind);
    }

    [TestMethod]
    public void SetModified_UpdatesLastModifiedToUtcNow()
    {
        TestRecord record = new();
        DateTime before = DateTime.UtcNow;
        record.SetModified();
        DateTime after = DateTime.UtcNow;

        Assert.AreEqual(DateTimeKind.Utc, record.LastModified.Kind);
        Assert.IsTrue(record.LastModified >= before && record.LastModified <= after);
    }
}
