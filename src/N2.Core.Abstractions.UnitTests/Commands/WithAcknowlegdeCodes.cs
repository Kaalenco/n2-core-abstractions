using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithAcknowlegdeCodes
{
    // -------------------------------------------------------------------------
    // Base flag values
    // -------------------------------------------------------------------------

    [TestMethod]
    public void BaseValuesMatchExpectedIntValues()
    {
        Assert.AreEqual(0, (int)AcknowledgeCodes.None);
        Assert.AreEqual(1, (int)AcknowledgeCodes.Yes);
        Assert.AreEqual(2, (int)AcknowledgeCodes.No);
        Assert.AreEqual(4, (int)AcknowledgeCodes.Ok);
        Assert.AreEqual(8, (int)AcknowledgeCodes.Cancel);
        Assert.AreEqual(16, (int)AcknowledgeCodes.Retry);
        Assert.AreEqual(32, (int)AcknowledgeCodes.Abort);
        Assert.AreEqual(64, (int)AcknowledgeCodes.Error);
    }

    // -------------------------------------------------------------------------
    // Composite flag values
    // -------------------------------------------------------------------------

    [TestMethod]
    public void CompositeValuesAreCorrectFlagCombinations()
    {
        Assert.AreEqual(AcknowledgeCodes.Yes | AcknowledgeCodes.No, AcknowledgeCodes.YesNo);
        Assert.AreEqual(AcknowledgeCodes.Ok | AcknowledgeCodes.Cancel, AcknowledgeCodes.OkCancel);
        Assert.AreEqual(AcknowledgeCodes.Cancel | AcknowledgeCodes.Retry, AcknowledgeCodes.CancelRetry);
        Assert.AreEqual(AcknowledgeCodes.Retry | AcknowledgeCodes.Abort, AcknowledgeCodes.RetryAbort);
        Assert.AreEqual(AcknowledgeCodes.Yes | AcknowledgeCodes.No | AcknowledgeCodes.Retry, AcknowledgeCodes.YesNoRetry);
    }

    [TestMethod]
    public void HasFlagWorksCorrectlyForComposites()
    {
        Assert.IsTrue(AcknowledgeCodes.OkCancel.HasFlag(AcknowledgeCodes.Ok));
        Assert.IsTrue(AcknowledgeCodes.OkCancel.HasFlag(AcknowledgeCodes.Cancel));
        Assert.IsFalse(AcknowledgeCodes.OkCancel.HasFlag(AcknowledgeCodes.Yes));
        Assert.IsFalse(AcknowledgeCodes.OkCancel.HasFlag(AcknowledgeCodes.Retry));

        Assert.IsTrue(AcknowledgeCodes.YesNoRetry.HasFlag(AcknowledgeCodes.Yes));
        Assert.IsTrue(AcknowledgeCodes.YesNoRetry.HasFlag(AcknowledgeCodes.No));
        Assert.IsTrue(AcknowledgeCodes.YesNoRetry.HasFlag(AcknowledgeCodes.Retry));
        Assert.IsFalse(AcknowledgeCodes.YesNoRetry.HasFlag(AcknowledgeCodes.Abort));
    }

    [TestMethod]
    public void NoneHasNoFlagsSet()
    {
        Assert.IsFalse(AcknowledgeCodes.None.HasFlag(AcknowledgeCodes.Yes));
        Assert.IsFalse(AcknowledgeCodes.None.HasFlag(AcknowledgeCodes.Ok));
        Assert.IsFalse(AcknowledgeCodes.None.HasFlag(AcknowledgeCodes.Cancel));
    }
}
