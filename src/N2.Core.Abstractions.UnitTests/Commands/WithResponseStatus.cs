using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithResponseStatus
{
    [TestMethod]
    public void SuccessGroupValuesMatchHttpStatusCodes()
    {
        Assert.AreEqual(200, (int)ResponseStatus.Success);
        Assert.AreEqual(201, (int)ResponseStatus.Created);
        Assert.AreEqual(202, (int)ResponseStatus.Accepted);
        Assert.AreEqual(203, (int)ResponseStatus.Information);
        Assert.AreEqual(204, (int)ResponseStatus.NoContent);
        Assert.AreEqual(205, (int)ResponseStatus.ResetContent);
    }

    [TestMethod]
    public void RedirectAndClientErrorGroupValuesMatchHttpStatusCodes()
    {
        Assert.AreEqual(300, (int)ResponseStatus.NotAccepted);
        Assert.AreEqual(304, (int)ResponseStatus.NotModified);
        Assert.AreEqual(400, (int)ResponseStatus.BadRequest);
        Assert.AreEqual(401, (int)ResponseStatus.Unauthorized);
        Assert.AreEqual(403, (int)ResponseStatus.Forbidden);
        Assert.AreEqual(404, (int)ResponseStatus.NotFound);
        Assert.AreEqual(405, (int)ResponseStatus.MethodNotAllowed);
        Assert.AreEqual(406, (int)ResponseStatus.NotAcceptable);
        Assert.AreEqual(408, (int)ResponseStatus.TimeOut);
        Assert.AreEqual(409, (int)ResponseStatus.Conflict);
        Assert.AreEqual(412, (int)ResponseStatus.PreconditionFailed);
        Assert.AreEqual(423, (int)ResponseStatus.Locked);
        Assert.AreEqual(429, (int)ResponseStatus.ToomanyRequests);
    }

    [TestMethod]
    public void ServerErrorGroupValuesMatchHttpStatusCodes()
    {
        Assert.AreEqual(500, (int)ResponseStatus.ServerError);
        Assert.AreEqual(501, (int)ResponseStatus.NotImplemented);
        Assert.AreEqual(503, (int)ResponseStatus.ServiceUnavailable);
    }

    [TestMethod]
    public void NoneValueIsZero()
    {
        Assert.AreEqual(0, (int)ResponseStatus.None);
    }
}
