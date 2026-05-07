using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class UsingSelectOptionsExtensions
{
    [TestMethod]
    public void Ok_ReturnsTrueOnlyForOkOption()
    {
        Assert.IsTrue(SelectOption.Ok.Ok());
        Assert.IsFalse(SelectOption.None.Ok());
        Assert.IsFalse(SelectOption.Cancel.Ok());
        Assert.IsFalse(SelectOption.Retry.Ok());
    }

    [TestMethod]
    public void Cancel_ReturnsTrueOnlyForCancelOption()
    {
        Assert.IsTrue(SelectOption.Cancel.Cancel());
        Assert.IsFalse(SelectOption.None.Cancel());
        Assert.IsFalse(SelectOption.Ok.Cancel());
        Assert.IsFalse(SelectOption.Retry.Cancel());
    }

    [TestMethod]
    public void Retry_ReturnsTrueOnlyForRetryOption()
    {
        Assert.IsTrue(SelectOption.Retry.Retry());
        Assert.IsFalse(SelectOption.None.Retry());
        Assert.IsFalse(SelectOption.Ok.Retry());
        Assert.IsFalse(SelectOption.Cancel.Retry());
    }

    [TestMethod]
    public void Ignore_ReturnsTrueOnlyForIgnoreOption()
    {
        Assert.IsTrue(SelectOption.Ignore.Ignore());
        Assert.IsFalse(SelectOption.None.Ignore());
        Assert.IsFalse(SelectOption.Ok.Ignore());
        Assert.IsFalse(SelectOption.Cancel.Ignore());
    }
}
