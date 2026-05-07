using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserRight
{
    [TestMethod]
    public void DefaultCtorSetsEmptyGuidAndFalseFlags()
    {
        UserRight right = new();
        Assert.AreEqual(Guid.Empty, right.UserId);
        Assert.AreEqual(string.Empty, right.DisplayName);
        Assert.IsFalse(right.Create);
        Assert.IsFalse(right.Read);
        Assert.IsFalse(right.Update);
        Assert.IsFalse(right.Delete);
        Assert.IsFalse(right.Design);
        Assert.IsFalse(right.RemoveThisRecord);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Guid userId = Guid.NewGuid();
        UserRight right = new()
        {
            UserId = userId,
            DisplayName = "Alice",
            Create = true,
            Read = true,
            Update = true,
            Delete = true,
            Design = true,
            RemoveThisRecord = true
        };

        Assert.AreEqual(userId, right.UserId);
        Assert.AreEqual("Alice", right.DisplayName);
        Assert.IsTrue(right.Create);
        Assert.IsTrue(right.Read);
        Assert.IsTrue(right.Update);
        Assert.IsTrue(right.Delete);
        Assert.IsTrue(right.Design);
        Assert.IsTrue(right.RemoveThisRecord);
    }

    [TestMethod]
    public void ToStringReturnsDisplayName()
    {
        UserRight right = new() { DisplayName = "Bob" };
        Assert.AreEqual("Bob", right.ToString());
    }
}
