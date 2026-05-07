using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserAccessModel
{
    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_RoundTrip_PreservesAllProperties()
    {
        var dto = new UserAccessModel
        {
            // TODO: assign every public property with explicit, culture-neutral values.
            // DateTime: always use DateTimeKind.Utc — Local and Unspecified do not
            // survive a System.Text.Json round-trip with their Kind intact.
            //   e.g. CreatedAt = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc)
        };

        var json   = JsonSerializer.Serialize(dto);
        var result = JsonSerializer.Deserialize<UserAccessModel>(json);

        Assert.IsNotNull(result);
        // TODO: assert each property by value.
        // For DateTime properties assert both value and Kind to catch locale drift:
        //   Assert.AreEqual(dto.CreatedAt, result.CreatedAt);
        //   Assert.AreEqual(DateTimeKind.Utc, result.CreatedAt.Kind);
    }

    [TestMethod]
    public void Deserialization_EmptyJson_ReturnsDefaultInstance()
    {
        var result = JsonSerializer.Deserialize<UserAccessModel>("{}");
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Deserialization_UnknownProperties_AreIgnored()
    {
        var json   = "{\"__unknown\": 42}";
        var result = JsonSerializer.Deserialize<UserAccessModel>(json);
        Assert.IsNotNull(result);
    }
}
