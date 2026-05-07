using Microsoft.VisualStudio.TestTools.UnitTesting;
using N2.Core.Commands;

namespace N2.Core.Entity;

[TestClass]
public sealed class WithDataContextHealthStatus
{
    [TestMethod]
    public void SuccessStatus_SetsIsHealthyTrueAndStatusCode200()
    {
        DataContextHealthStatus status = new(ResponseStatus.Success, "OK");
        Assert.IsTrue(status.IsHealthy);
        Assert.AreEqual(200, status.StatusCode);
        Assert.AreEqual("OK", status.Message);
    }

    [TestMethod]
    public void ErrorStatus_SetsIsHealthyFalseAndExpectedStatusCode()
    {
        DataContextHealthStatus status = new(ResponseStatus.ServerError, "Database unreachable");
        Assert.IsFalse(status.IsHealthy);
        Assert.AreEqual(500, status.StatusCode);
        Assert.AreEqual("Database unreachable", status.Message);
    }
}
