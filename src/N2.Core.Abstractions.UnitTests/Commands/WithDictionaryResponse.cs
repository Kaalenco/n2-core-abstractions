using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithDictionaryResponse
{
    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    [TestMethod]
    public void DefaultCtorCreatesEmptyDictionary()
    {
        DictionaryResponse r = new();
        Assert.IsNotNull(r.Value);
        Assert.AreEqual(0, r.Value.Count);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
    }

    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_RoundTrip_PreservesValueAndStatus()
    {
        DictionaryResponse dto = new();
        dto.Value!["name"] = "test";

        string json = JsonSerializer.Serialize(dto);
        DictionaryResponse? result = JsonSerializer.Deserialize<DictionaryResponse>(json);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResponseStatus.Success, result.Status);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.ContainsKey("name"));
        Assert.AreEqual("test", ((JsonElement)result.Value["name"]).GetString());
    }

    [TestMethod]
    public void Deserialization_EmptyJson_ValueIsNotNull()
    {
        DictionaryResponse? result = JsonSerializer.Deserialize<DictionaryResponse>("{}");
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
    }

    [TestMethod]
    public void Deserialization_UnknownProperties_AreIgnored()
    {
        DictionaryResponse? result = JsonSerializer.Deserialize<DictionaryResponse>("{\"__unknown\": 42}");
        Assert.IsNotNull(result);
    }
}
