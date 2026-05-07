using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithTokenRequest
{
    [TestMethod]
    public void DefaultCtorSetsEmptyAudienceAndNullValue()
    {
        TokenRequest request = new();
        Assert.AreEqual(string.Empty, request.Audience);
        Assert.IsNull(request.Value);
        Assert.IsNull(request.Handle);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Token token = new() { GrantType = "password" };
        TokenRequest request = new()
        {
            Audience = "api.example.com",
            Value = token,
            Handle = "req-001"
        };

        Assert.AreEqual("api.example.com", request.Audience);
        Assert.AreEqual(token, request.Value);
        Assert.AreEqual("req-001", request.Handle);
    }
}
