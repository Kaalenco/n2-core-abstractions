using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

[TestClass]
public class WithResponseStatusExtensions
{
    [TestMethod]
    public void IsSuccessReturnsTrueForAllSuccessStatuses()
    {
        Assert.IsTrue(ResponseStatus.Success.IsSuccess());
        Assert.IsTrue(ResponseStatus.Created.IsSuccess());
        Assert.IsTrue(ResponseStatus.Accepted.IsSuccess());
        Assert.IsTrue(ResponseStatus.Information.IsSuccess());
        Assert.IsTrue(ResponseStatus.NoContent.IsSuccess());
        Assert.IsTrue(ResponseStatus.ResetContent.IsSuccess());
    }

    [TestMethod]
    public void IsSuccessReturnsFalseForErrorStatuses()
    {
        Assert.IsFalse(ResponseStatus.NotAccepted.IsSuccess());
        Assert.IsFalse(ResponseStatus.BadRequest.IsSuccess());
        Assert.IsFalse(ResponseStatus.Unauthorized.IsSuccess());
        Assert.IsFalse(ResponseStatus.Forbidden.IsSuccess());
        Assert.IsFalse(ResponseStatus.NotFound.IsSuccess());
        Assert.IsFalse(ResponseStatus.ServerError.IsSuccess());
        Assert.IsFalse(ResponseStatus.ServiceUnavailable.IsSuccess());
    }

    [TestMethod]
    public void ToMessageReturnsCorrectTextForSuccessStatuses()
    {
        Assert.AreEqual("The operation was successful.", ResponseStatus.Success.ToMessage());
        Assert.AreEqual("The resource was created successfully.", ResponseStatus.Created.ToMessage());
        Assert.AreEqual("The request has been accepted for processing.", ResponseStatus.Accepted.ToMessage());
        Assert.AreEqual("The request was accepted and is being processed.", ResponseStatus.Information.ToMessage());
        Assert.AreEqual("The request was successful but there is no content to return.", ResponseStatus.NoContent.ToMessage());
        Assert.AreEqual("The content has been reset successfully.", ResponseStatus.ResetContent.ToMessage());
    }

    [TestMethod]
    public void ToMessageReturnsCorrectTextForErrorStatuses()
    {
        Assert.AreEqual("The request was not accepted.", ResponseStatus.NotAccepted.ToMessage());
        Assert.AreEqual("The request was invalid or cannot be served.", ResponseStatus.BadRequest.ToMessage());
        Assert.AreEqual("Authentication is required and has failed or has not yet been provided.", ResponseStatus.Unauthorized.ToMessage());
        Assert.AreEqual("The requested resource could not be found.", ResponseStatus.NotFound.ToMessage());
        Assert.AreEqual("An internal server error occurred while processing the request.", ResponseStatus.ServerError.ToMessage());
        Assert.AreEqual("The service is currently unavailable, please try again later.", ResponseStatus.ServiceUnavailable.ToMessage());
    }

    [TestMethod]
    public void ToMessageReturnsUnknownForUndefinedStatus()
    {
        ResponseStatus unknown = (ResponseStatus)999;
        Assert.AreEqual("Unknown response status.", unknown.ToMessage());
    }
}
