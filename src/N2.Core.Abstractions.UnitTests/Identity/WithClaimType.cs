using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithClaimType
{
    [TestMethod]
    public void NoneIsZero()
    {
        Assert.AreEqual(0, (int)ClaimType.none);
    }

    [TestMethod]
    public void ValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(1, (int)ClaimType.UserName);
        Assert.AreEqual(2, (int)ClaimType.Email);
        Assert.AreEqual(3, (int)ClaimType.Role);
        Assert.AreEqual(4, (int)ClaimType.Actor);
        Assert.AreEqual(5, (int)ClaimType.PrimarySid);
        Assert.AreEqual(6, (int)ClaimType.GivenName);
        Assert.AreEqual(7, (int)ClaimType.Culture);
    }
}
