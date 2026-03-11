using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Identity;

namespace N2.Core.Abstractions.UnitTests.Identity;

[TestClass]
public class WithToken
{
    [TestMethod]
    public void TokenShouldSerialize()
    {
        Token item = new();
        string json = item.Serialize();
        Assert.IsNotNull(json);
        Assert.AreEqual("{\"grant_type\":\"none\"}", json);
    }

    [TestMethod]
    public void TokenShouldDeserialize()
    {
        string json = "{\"grant_type\":\"basic\", \"access_token\": \"user:password\"}";
        Token item = Token.Deserialize(json);
        Assert.IsNotNull(item);
        Assert.AreEqual("basic", item.GrantType);
        Assert.AreEqual("user:password", item.AccessToken);
    }

    [TestMethod]
    public void TokenDeserializeThrowsException()
    {
        string json = "{\"access_token\": \"user:password\"}";

        Assert.Throws<JsonException>(() => Token.Deserialize(json));
    }
}