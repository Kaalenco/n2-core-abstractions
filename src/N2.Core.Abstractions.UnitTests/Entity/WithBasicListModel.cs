using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Entity;

[TestClass]
public sealed class WithBasicListModel
{
    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_RoundTrip_PreservesAllProperties()
    {
        var dto = new BasicListTestModel
        {
            // TODO: assign every public property with explicit, culture-neutral values.
            // DateTime: always use DateTimeKind.Utc — Local and Unspecified do not
            // survive a System.Text.Json round-trip with their Kind intact.
            //   e.g. CreatedAt = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc)
        };

        var json   = JsonSerializer.Serialize(dto);
        var result = JsonSerializer.Deserialize<BasicListTestModel>(json);

        Assert.IsNotNull(result);
        // TODO: assert each property by value.
        // For DateTime properties assert both value and Kind to catch locale drift:
        //   Assert.AreEqual(dto.CreatedAt, result.CreatedAt);
        //   Assert.AreEqual(DateTimeKind.Utc, result.CreatedAt.Kind);
    }

    [TestMethod]
    public void Serialization_DateTimeProperties_KindIsPreserved()
    {
        // System.Text.Json round-trips DateTimeKind.Utc correctly (appends 'Z').
        // DateTimeKind.Local becomes Unspecified on deserialize — use Utc in DTOs.
        var utcValue = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        var dto = new BasicListTestModel
        {
            Created = utcValue
        };

        var json   = JsonSerializer.Serialize(dto);
        var result = JsonSerializer.Deserialize<BasicListTestModel>(json);

        Assert.IsNotNull(result);

        Assert.AreEqual(utcValue, result.Created);
        Assert.AreEqual(DateTimeKind.Utc, result.Created.Kind);
    }

    [TestMethod]
    public void Deserialization_EmptyJson_ReturnsDefaultInstance()
    {
        var result = JsonSerializer.Deserialize<BasicListTestModel>("{}");
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Deserialization_UnknownProperties_AreIgnored()
    {
        var json   = "{\"__unknown\": 42}";
        var result = JsonSerializer.Deserialize<BasicListTestModel>(json);
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