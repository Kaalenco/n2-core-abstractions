using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithSelectOption
{
    [TestMethod]
    public void BaseValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(0, (int)SelectOption.None);
        Assert.AreEqual(1, (int)SelectOption.Ok);
        Assert.AreEqual(2, (int)SelectOption.Cancel);
        Assert.AreEqual(4, (int)SelectOption.Retry);
        Assert.AreEqual(8, (int)SelectOption.Ignore);
    }

    [TestMethod]
    public void CompositeValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(3, (int)SelectOption.CancelOk);
        Assert.AreEqual(5, (int)SelectOption.RetryOk);
        Assert.AreEqual(6, (int)SelectOption.RetryCancel);
        Assert.AreEqual(7, (int)SelectOption.RetryCancelOk);
        Assert.AreEqual(9, (int)SelectOption.IgnoreOk);
        Assert.AreEqual(10, (int)SelectOption.IgnoreCancel);
        Assert.AreEqual(11, (int)SelectOption.IgnoreCancelOk);
        Assert.AreEqual(12, (int)SelectOption.IgnoreRetry);
        Assert.AreEqual(13, (int)SelectOption.IgnoreRetryOk);
        Assert.AreEqual(14, (int)SelectOption.IgnoreRetryCancel);
        Assert.AreEqual(15, (int)SelectOption.IgnoreRetryCancelOk);
    }
}
