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
}