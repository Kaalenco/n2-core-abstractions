using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Entity;

[TestClass]
public sealed class WithBasicListModel
{
    [TestMethod]
    public void DefaultCtorInitializesScalarDefaults()
    {
        BasicListTestModel model = new();
        Assert.AreEqual(Guid.Empty, model.OwnerId);
        Assert.AreEqual(string.Empty, model.OwnerName);
        Assert.IsFalse(model.DbItem);
        Assert.AreEqual(Guid.Empty, model.PublicId);
        Assert.AreEqual(string.Empty, model.ValidationMessage);
        Assert.AreEqual(Guid.Empty, model.CreatedBy);
        Assert.AreEqual(Guid.Empty, model.ModifiedBy);
    }

    [TestMethod]
    public void AbstractPropertiesReturnValuesFromConcreteImplementation()
    {
        BasicListTestModel model = new();
        Assert.AreEqual("BasicListTestModel", model.Name);
        Assert.AreEqual("Test class for lists", model.Description);
        Assert.IsTrue(model.IsActive);
    }

    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_RoundTrip_PreservesAllProperties()
    {
        Guid ownerId = Guid.NewGuid();
        Guid publicId = Guid.NewGuid();
        DateTime created = new(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        DateTime modified = new(2024, 6, 20, 8, 0, 0, DateTimeKind.Utc);

        BasicListTestModel dto = new()
        {
            OwnerId = ownerId,
            OwnerName = "Test Owner",
            DbItem = true,
            PublicId = publicId,
            Created = created,
            Modified = modified,
            ValidationMessage = "test"
        };

        string json = JsonSerializer.Serialize(dto);
        BasicListTestModel? result = JsonSerializer.Deserialize<BasicListTestModel>(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(ownerId, result.OwnerId);
        Assert.AreEqual("Test Owner", result.OwnerName);
        Assert.IsTrue(result.DbItem);
        Assert.AreEqual(publicId, result.PublicId);
        Assert.AreEqual(created, result.Created);
        Assert.AreEqual(DateTimeKind.Utc, result.Created.Kind);
        Assert.AreEqual(modified, result.Modified);
        Assert.AreEqual(DateTimeKind.Utc, result.Modified.Kind);
        Assert.AreEqual("test", result.ValidationMessage);
    }

    [TestMethod]
    public void Serialization_DateTimeProperties_KindIsPreserved()
    {
        DateTime utcValue = new(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        BasicListTestModel dto = new() { Created = utcValue };

        string json = JsonSerializer.Serialize(dto);
        BasicListTestModel? result = JsonSerializer.Deserialize<BasicListTestModel>(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(utcValue, result.Created);
        Assert.AreEqual(DateTimeKind.Utc, result.Created.Kind);
    }

    [TestMethod]
    public void Deserialization_EmptyJson_ReturnsDefaultInstance()
    {
        BasicListTestModel? result = JsonSerializer.Deserialize<BasicListTestModel>("{}");
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Deserialization_UnknownProperties_AreIgnored()
    {
        string json = "{\"__unknown\": 42}";
        BasicListTestModel? result = JsonSerializer.Deserialize<BasicListTestModel>(json);
        Assert.IsNotNull(result);
    }
}

public class BasicListTestModel : BasicListModel
{
    public override string Name => nameof(BasicListTestModel);
    public override string Description => "Test class for lists";
    public override bool IsActive => true;

    public override void Init(string Name, string Description, bool IsActive) => throw new NotImplementedException();
    public override bool IsValid() => throw new NotImplementedException();
}
