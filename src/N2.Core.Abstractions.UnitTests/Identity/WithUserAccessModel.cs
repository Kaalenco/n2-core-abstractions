using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserAccessModel
{
    [TestMethod]
    public void DefaultCtorInitializesEmptyCollectionsAndDefaultScalars()
    {
        UserAccessModel model = new();
        Assert.IsNotNull(model.RoleRights);
        Assert.AreEqual(0, model.RoleRights.Count);
        Assert.IsNotNull(model.UserRights);
        Assert.AreEqual(0, model.UserRights.Count);
        Assert.IsNotNull(model.Assigned);
        Assert.AreEqual(0, model.Assigned.Count);
        Assert.AreEqual(Guid.Empty, model.OwnerId);
        Assert.AreEqual(string.Empty, model.OwnerName);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Guid ownerId = Guid.NewGuid();
        UserAccessModel model = new()
        {
            OwnerId = ownerId,
            OwnerName = "Alice"
        };

        Assert.AreEqual(ownerId, model.OwnerId);
        Assert.AreEqual("Alice", model.OwnerName);
    }

    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_RoundTrip_PreservesScalarProperties()
    {
        Guid ownerId = Guid.NewGuid();
        UserAccessModel dto = new()
        {
            OwnerId = ownerId,
            OwnerName = "Bob"
        };

        string json = JsonSerializer.Serialize(dto);
        UserAccessModel? result = JsonSerializer.Deserialize<UserAccessModel>(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(ownerId, result.OwnerId);
        Assert.AreEqual("Bob", result.OwnerName);
    }

    [TestMethod]
    public void Deserialization_EmptyJson_ReturnsDefaultInstance()
    {
        UserAccessModel? result = JsonSerializer.Deserialize<UserAccessModel>("{}");
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Deserialization_UnknownProperties_AreIgnored()
    {
        string json = "{\"__unknown\": 42}";
        UserAccessModel? result = JsonSerializer.Deserialize<UserAccessModel>(json);
        Assert.IsNotNull(result);
    }
}
