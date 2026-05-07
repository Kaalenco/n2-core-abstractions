using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserLogin
{
    [TestMethod]
    public void DefaultCtorSetsEmptyUsernameAndPassword()
    {
        UserLogin login = new();
        Assert.AreEqual(string.Empty, login.Username);
        Assert.AreEqual(string.Empty, login.Password);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        UserLogin login = new()
        {
            Username = "alice",
            Password = "s3cr3t"
        };

        Assert.AreEqual("alice", login.Username);
        Assert.AreEqual("s3cr3t", login.Password);
    }
}
