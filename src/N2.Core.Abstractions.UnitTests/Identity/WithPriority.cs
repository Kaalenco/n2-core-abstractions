using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithPriority
{
    [TestMethod]
    public void ValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(0, (int)Priority.Low);
        Assert.AreEqual(1, (int)Priority.Normal);
        Assert.AreEqual(2, (int)Priority.High);
    }
}
