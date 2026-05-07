using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithAcknowledgeResponse
{
    // -------------------------------------------------------------------------
    // Constructors
    // -------------------------------------------------------------------------

    [TestMethod]
    public void DefaultCtorSetsCodeNoneAndSuccessStatus()
    {
        AcknowledgeResponse r = new();
        Assert.AreEqual(AcknowledgeCodes.None, r.Code);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
        Assert.IsNull(r.Message);
    }

    [TestMethod]
    public void CodeMessageCtorSetsBothAndSuccessStatus()
    {
        AcknowledgeResponse r = new(AcknowledgeCodes.Ok, "confirmed");
        Assert.AreEqual(AcknowledgeCodes.Ok, r.Code);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
        Assert.AreEqual("confirmed", r.Message);
    }

    [TestMethod]
    public void CodeOnlyCtorSetsCodeAndSuccessStatus()
    {
        AcknowledgeResponse r = new(AcknowledgeCodes.Cancel);
        Assert.AreEqual(AcknowledgeCodes.Cancel, r.Code);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
        Assert.IsNull(r.Message);
    }

    [TestMethod]
    public void CodePropertyIsSettable()
    {
        AcknowledgeResponse r = new();
        r.Code = AcknowledgeCodes.YesNo;
        Assert.AreEqual(AcknowledgeCodes.YesNo, r.Code);
    }
}
