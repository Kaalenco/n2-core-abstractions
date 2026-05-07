using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class UsingGrantType
{
    [TestMethod]
    public void AllConstantsHaveExpectedValues()
    {
        Assert.AreEqual("client_credentials", GrantType.ClientCredentials);
        Assert.AreEqual("password", GrantType.Password);
        Assert.AreEqual("authorization_code", GrantType.AuthorizationCode);
        Assert.AreEqual("refresh_token", GrantType.RefreshToken);
        Assert.AreEqual("basic", GrantType.Basic);
        Assert.AreEqual("access_token", GrantType.AccessToken);
        Assert.AreEqual("refresh", GrantType.Refresh);
        Assert.AreEqual("logoff_user", GrantType.LogoffUser);
        Assert.AreEqual("logoff", GrantType.Logoff);
    }
}
