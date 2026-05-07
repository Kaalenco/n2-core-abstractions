using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2.Core.Commands;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithTokenResponse
{
    [TestMethod]
    public void DefaultCtorSetsNullValueAndSuccessStatus()
    {
        TokenResponse response = new();
        Assert.IsNull(response.Value);
        Assert.AreEqual(ResponseStatus.Success, response.Status);
    }

    [TestMethod]
    public void TokenCtorSetsValueAndSuccessStatus()
    {
        Token token = new() { GrantType = "password" };
        TokenResponse response = new(token);
        Assert.AreEqual(token, response.Value);
        Assert.AreEqual(ResponseStatus.Success, response.Status);
    }

    [TestMethod]
    public void TokenStatusCtorSetsValueAndProvidedStatus()
    {
        Token token = new() { GrantType = "basic" };
        TokenResponse response = new(token, ResponseStatus.Unauthorized);
        Assert.AreEqual(token, response.Value);
        Assert.AreEqual(ResponseStatus.Unauthorized, response.Status);
    }

    [TestMethod]
    public void Failed_SetsStatusAndMessage()
    {
        TokenResponse response = TokenResponse.Failed(ResponseStatus.Unauthorized, "Invalid credentials");
        Assert.IsNull(response.Value);
        Assert.AreEqual(ResponseStatus.Unauthorized, response.Status);
        Assert.AreEqual("Invalid credentials", response.Message);
    }
}
