using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserAlert
{
    [TestMethod]
    public void DefaultCtorSetsUtcTimeAndEmptyMessageAndLowPriority()
    {
        DateTime before = DateTime.UtcNow;
        UserAlert alert = new();
        DateTime after = DateTime.UtcNow;

        Assert.AreEqual(DateTimeKind.Utc, alert.Time.Kind);
        Assert.IsTrue(alert.Time >= before && alert.Time <= after);
        Assert.AreEqual(string.Empty, alert.Message);
        Assert.AreEqual(Priority.Low, alert.Priority);
    }

    [TestMethod]
    public void MessagePriorityCtorSetsAllProperties()
    {
        DateTime before = DateTime.UtcNow;
        UserAlert alert = new("Server error", Priority.High);
        DateTime after = DateTime.UtcNow;

        Assert.AreEqual(DateTimeKind.Utc, alert.Time.Kind);
        Assert.IsTrue(alert.Time >= before && alert.Time <= after);
        Assert.AreEqual("Server error", alert.Message);
        Assert.AreEqual(Priority.High, alert.Priority);
    }
}
