using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserProfile
{
    [TestMethod]
    public void DefaultCtorSetsEmptyStringsAndNullOptionals()
    {
        UserProfile profile = new();
        Assert.AreEqual(Guid.Empty, profile.Id);
        Assert.AreEqual(string.Empty, profile.Username);
        Assert.AreEqual(string.Empty, profile.Password);
        Assert.AreEqual(string.Empty, profile.Email);
        Assert.AreEqual(string.Empty, profile.PhoneNumber);
        Assert.IsNull(profile.FirstName);
        Assert.IsNull(profile.LastName);
        Assert.IsNull(profile.MiddleName);
        Assert.IsNull(profile.FullName);
        Assert.IsNull(profile.Avatar);
        Assert.IsNull(profile.DisplayName);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Guid id = Guid.NewGuid();
        UserProfile profile = new()
        {
            Id = id,
            Username = "jdoe",
            Password = "pass",
            Email = "jdoe@example.com",
            PhoneNumber = "+31600000000",
            FirstName = "John",
            LastName = "Doe",
            DisplayName = "John Doe"
        };

        Assert.AreEqual(id, profile.Id);
        Assert.AreEqual("jdoe", profile.Username);
        Assert.AreEqual("jdoe@example.com", profile.Email);
        Assert.AreEqual("+31600000000", profile.PhoneNumber);
        Assert.AreEqual("John", profile.FirstName);
        Assert.AreEqual("Doe", profile.LastName);
        Assert.AreEqual("John Doe", profile.DisplayName);
    }
}
