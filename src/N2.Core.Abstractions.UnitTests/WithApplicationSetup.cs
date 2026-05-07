using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithApplicationSetup
{
    [TestMethod]
    public void DefaultCtorInitializesEmptyStringsAndEmptyRoles()
    {
        ApplicationSetup setup = new();
        Assert.AreEqual(string.Empty, setup.Name);
        Assert.AreEqual(string.Empty, setup.Version);
        Assert.AreEqual(string.Empty, setup.Description);
        Assert.AreEqual(string.Empty, setup.Logo);
        Assert.AreEqual(string.Empty, setup.AdminEmail);
        Assert.AreEqual(string.Empty, setup.SupportEmail);
        Assert.AreEqual(string.Empty, setup.OwnerEmail);
        Assert.AreEqual(string.Empty, setup.OwnerName);
        Assert.IsNotNull(setup.Roles);
        Assert.AreEqual(0, setup.Roles.Length);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        ApplicationSetup setup = new()
        {
            Name = "MyApp",
            Version = "2.0",
            Description = "Test application",
            Logo = "/logo.png",
            AdminEmail = "admin@example.com",
            SupportEmail = "support@example.com",
            OwnerEmail = "owner@example.com",
            OwnerName = "Owner",
            Roles = ["admin", "user"]
        };

        Assert.AreEqual("MyApp", setup.Name);
        Assert.AreEqual("2.0", setup.Version);
        Assert.AreEqual("Test application", setup.Description);
        Assert.AreEqual(2, setup.Roles.Length);
        Assert.AreEqual("admin", setup.Roles[0]);
    }
}
