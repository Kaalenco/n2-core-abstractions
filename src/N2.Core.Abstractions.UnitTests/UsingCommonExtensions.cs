using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class UsingCommonExtensions
{
    [TestMethod]
    public void StaticOverloadReturnsJan1st1970Utc()
    {
        DateTime epoch = CommonExtensions.UnixEpoch();
        Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), epoch);
        Assert.AreEqual(DateTimeKind.Utc, epoch.Kind);
    }

    [TestMethod]
    public void ExtensionOverloadReturnsSameValueAsStatic()
    {
        DateTime someDate = new DateTime(2024, 6, 15, 12, 0, 0);
        DateTime epoch = someDate.UnixEpoch();
        Assert.AreEqual(CommonExtensions.UnixEpoch(), epoch);
        Assert.AreEqual(DateTimeKind.Utc, epoch.Kind);
    }
}
