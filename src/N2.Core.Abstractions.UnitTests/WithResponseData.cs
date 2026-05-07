using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithResponseData
{
    // -------------------------------------------------------------------------
    // ResponseData
    // -------------------------------------------------------------------------

    [TestMethod]
    public void DefaultCtorResultIsNull()
    {
        ResponseData data = new();
        Assert.IsNull(data.Result);
    }

    [TestMethod]
    public void CreateFactory_SetsResultAndData()
    {
        RequestResult result = RequestResult.Ok();
        ResponseData<string> created = ResponseData.Create(result, "payload");

        Assert.AreEqual(result, created.Result);
        Assert.AreEqual("payload", created.Data);
    }

    [TestMethod]
    public void CreateFactory_WithoutData_DataIsDefault()
    {
        RequestResult result = RequestResult.NotFound();
        ResponseData<string> created = ResponseData.Create<string>(result);

        Assert.AreEqual(result, created.Result);
        Assert.IsNull(created.Data);
    }

    // -------------------------------------------------------------------------
    // ResponseData<TValue>
    // -------------------------------------------------------------------------

    [TestMethod]
    public void GenericType_DefaultCtorPropertiesAreNull()
    {
        ResponseData<string> data = new();
        Assert.IsNull(data.Result);
        Assert.IsNull(data.Data);
    }
}
