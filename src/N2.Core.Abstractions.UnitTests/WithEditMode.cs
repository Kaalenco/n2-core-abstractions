using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithEditMode
{
    [TestMethod]
    public void ValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(0, (int)EditMode.Hide);
        Assert.AreEqual(1, (int)EditMode.AddNew);
        Assert.AreEqual(2, (int)EditMode.Modify);
        Assert.AreEqual(3, (int)EditMode.Remove);
        Assert.AreEqual(4, (int)EditMode.Replicate);
    }
}
