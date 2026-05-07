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
        Assert.AreEqual("The resource has not been modified since the last request.", ResponseStatus.NotModified.ToMessage());
        Assert.AreEqual("The request was invalid or cannot be served.", ResponseStatus.BadRequest.ToMessage());
        Assert.AreEqual("Authentication is required and has failed or has not yet been provided.", ResponseStatus.Unauthorized.ToMessage());
        Assert.AreEqual("The server understood the request, but refuses to authorize it.", ResponseStatus.Forbidden.ToMessage());
        Assert.AreEqual("The requested resource could not be found.", ResponseStatus.NotFound.ToMessage());
        Assert.AreEqual("The requested resource is not available in a format acceptable to the client.", ResponseStatus.NotAcceptable.ToMessage());
        Assert.AreEqual("The server timed out waiting for the request.", ResponseStatus.TimeOut.ToMessage());
        Assert.AreEqual("There was a conflict with the current state of the resource.", ResponseStatus.Conflict.ToMessage());
        Assert.AreEqual("The resource is locked and cannot be modified at this time.", ResponseStatus.Locked.ToMessage());
        Assert.AreEqual("Too many requests have been made in a given amount of time.", ResponseStatus.ToomanyRequests.ToMessage());
        Assert.AreEqual("An internal server error occurred while processing the request.", ResponseStatus.ServerError.ToMessage());
        Assert.AreEqual("The server does not support the functionality required to fulfill the request.", ResponseStatus.NotImplemented.ToMessage());
        Assert.AreEqual("The service is currently unavailable, please try again later.", ResponseStatus.ServiceUnavailable.ToMessage());
    }

    [TestMethod]
    public void ToMessageReturnsUnknownForUndefinedStatus()
    {
        Assert.AreEqual("Unknown response status.", ((ResponseStatus)999).ToMessage());
    }

    [TestMethod]
    public void ToMessageReturnsUnknownForUnhandledDefinedStatuses()
    {
        Assert.AreEqual("Unknown response status.", ResponseStatus.None.ToMessage());
        Assert.AreEqual("Unknown response status.", ResponseStatus.MethodNotAllowed.ToMessage());
        Assert.AreEqual("Unknown response status.", ResponseStatus.PreconditionFailed.ToMessage());
    }
}
