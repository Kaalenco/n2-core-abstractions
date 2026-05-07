using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class UsingSystemRoles
{
    [TestMethod]
    public void AllConstantsHaveExpectedValues()
    {
        Assert.AreEqual("admin", SystemRoles.Admin);
        Assert.AreEqual("superadmin", SystemRoles.SuperAdmin);
        Assert.AreEqual("application", SystemRoles.Application);
        Assert.AreEqual("authmanager", SystemRoles.AuthManager);
        Assert.AreEqual("designer", SystemRoles.Designer);
        Assert.AreEqual("publisher", SystemRoles.Publisher);
        Assert.AreEqual("sysadmin", SystemRoles.SysAdmin);
        Assert.AreEqual("user", SystemRoles.User);
        Assert.AreEqual("visitor", SystemRoles.Visitor);
    }
}
