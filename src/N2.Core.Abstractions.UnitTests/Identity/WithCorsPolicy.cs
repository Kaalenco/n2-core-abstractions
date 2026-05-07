using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithCorsPolicy
{
    [TestMethod]
    public void DefaultCtorSetsNullNameAndNullOrigins()
    {
        CorsPolicy policy = new();
        Assert.IsNull(policy.Name);
        Assert.IsNull(policy.Origins);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        CorsPolicy policy = new()
        {
            Name = "AllowAll",
            Origins = ["https://example.com", "https://api.example.com"]
        };

        Assert.AreEqual("AllowAll", policy.Name);
        Assert.IsNotNull(policy.Origins);
        Assert.AreEqual(2, policy.Origins.Length);
        Assert.AreEqual("https://example.com", policy.Origins[0]);
    }
}
