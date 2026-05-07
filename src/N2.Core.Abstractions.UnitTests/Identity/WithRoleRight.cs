using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithRoleRight
{
    [TestMethod]
    public void DefaultCtorSetsEmptyRoleAndFalseFlags()
    {
        RoleRight right = new();
        Assert.AreEqual(string.Empty, right.Role);
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
        RoleRight right = new()
        {
            Role = "admin",
            Create = true,
            Read = true,
            Update = true,
            Delete = true,
            Design = true,
            RemoveThisRecord = true
        };

        Assert.AreEqual("admin", right.Role);
        Assert.IsTrue(right.Create);
        Assert.IsTrue(right.Read);
        Assert.IsTrue(right.Update);
        Assert.IsTrue(right.Delete);
        Assert.IsTrue(right.Design);
        Assert.IsTrue(right.RemoveThisRecord);
    }

    [TestMethod]
    public void ToStringReturnsRole()
    {
        RoleRight right = new() { Role = "editor" };
        Assert.AreEqual("editor", right.ToString());
    }
}
