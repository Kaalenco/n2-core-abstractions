using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests;

[TestClass]
public class WithRequestResultFactories
{
    [TestMethod]
    public void OkReturnsSingletonWithCode200()
    {
        RequestResult result = RequestResult.Ok();
        Assert.AreEqual(RequestResult.OkCode, result.Code);
        Assert.IsTrue(result.IsSuccessCode);
    }

    [TestMethod]
    public void OkWithMessageCreatesNewInstanceWithCustomMessage()
    {
        RequestResult result = RequestResult.Ok("custom");
        Assert.AreEqual(RequestResult.OkCode, result.Code);
        Assert.AreEqual("custom", result.Message);
        Assert.IsTrue(result.IsSuccessCode);
    }

    [TestMethod]
    public void AcceptedReturnsSingletonWithCode203()
    {
        RequestResult result = RequestResult.Accepted();
        Assert.AreEqual(RequestResult.AcceptedCode, result.Code);
        Assert.IsTrue(result.IsSuccessCode);
    }

    [TestMethod]
    public void BadRequestReturnsSingletonWithCode406()
    {
        RequestResult result = RequestResult.BadRequest();
        Assert.AreEqual(RequestResult.BadRequestCode, result.Code);
        Assert.IsFalse(result.IsSuccessCode);
    }

    [TestMethod]
    public void BadRequestWithMessageCreatesNewInstance()
    {
        RequestResult result = RequestResult.BadRequest("invalid input");
        Assert.AreEqual(RequestResult.BadRequestCode, result.Code);
        Assert.AreEqual("invalid input", result.Message);
    }

    [TestMethod]
    public void NotFoundReturnsSingletonWithCode404()
    {
        RequestResult result = RequestResult.NotFound();
        Assert.AreEqual(RequestResult.NotFoundCode, result.Code);
        Assert.IsFalse(result.IsSuccessCode);
    }

    [TestMethod]
    public void UnauthorizedReturnsSingletonWithCode403()
    {
        RequestResult result = RequestResult.Unauthorized();
        Assert.AreEqual(RequestResult.UnauthorizedCode, result.Code);
        Assert.IsFalse(result.IsSuccessCode);
    }

    [TestMethod]
    public void UnexpectedReturnsSingletonWithCode500()
    {
        RequestResult result = RequestResult.Unexpected();
        Assert.AreEqual(RequestResult.UnexpectedCode, result.Code);
        Assert.IsFalse(result.IsSuccessCode);
    }

    [TestMethod]
    public void TimeOutReturnsSingletonWithCode407()
    {
        RequestResult result = RequestResult.TimeOut();
        Assert.AreEqual(RequestResult.TimeOutCode, result.Code);
        Assert.IsFalse(result.IsSuccessCode);
    }

    [TestMethod]
    public void EqualityReturnsTrueForSameStatusAndMessage()
    {
        RequestResult a = new(RequestResult.OkCode, "hello");
        RequestResult b = new(RequestResult.OkCode, "hello");
        Assert.IsTrue(a == b);
        Assert.IsFalse(a != b);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void EqualityReturnsFalseForDifferentMessage()
    {
        RequestResult a = new(RequestResult.OkCode, "hello");
        RequestResult b = new(RequestResult.OkCode, "world");
        Assert.IsFalse(a == b);
        Assert.IsTrue(a != b);
    }

    [TestMethod]
    public void EqualityReturnsFalseForDifferentStatus()
    {
        RequestResult a = new(RequestResult.OkCode, "msg");
        RequestResult b = new(RequestResult.BadRequestCode, "msg");
        Assert.IsFalse(a == b);
    }

    [TestMethod]
    public void EqualsObjectReturnsTrueForSameCodeAsInt()
    {
        RequestResult result = new(RequestResult.OkCode, "msg");
        Assert.IsTrue(result.Equals((object)RequestResult.OkCode));
    }

    [TestMethod]
    public void EqualsObjectReturnsFalseForNull()
    {
        RequestResult result = RequestResult.Ok();
        Assert.IsFalse(result.Equals(null));
    }

    [TestMethod]
    public void CreateNewWithHandlePreservesHandle()
    {
        Guid handle = Guid.NewGuid();
        RequestResult original = RequestResult.Ok("original");
        ICommandResponse created = original.CreateNew(ResponseStatus.NoContent, "new message", handle);
        Assert.AreEqual(ResponseStatus.NoContent, created.Status);
        Assert.AreEqual("new message", created.Message);
        Assert.AreEqual(handle, created.Handle?.Value);
    }

    [TestMethod]
    public void CreateNewWithoutHandleOmitsHandle()
    {
        RequestResult original = RequestResult.Ok("original");
        ICommandResponse created = original.CreateNew(ResponseStatus.NoContent, "new message");
        Assert.IsNull(created.Handle);
    }

    [TestMethod]
    public void ClonePreservesAllProperties()
    {
        Guid handle = Guid.NewGuid();
        RequestResult original = new(ResponseStatus.Success, "hello", handle);
        object clone = original.Clone();
        Assert.IsInstanceOfType<RequestResult>(clone);
        RequestResult cloned = (RequestResult)clone;
        Assert.AreEqual(original.Status, cloned.Status);
        Assert.AreEqual(original.Message, cloned.Message);
    }

    [TestMethod]
    public void TupleConstructorSetsStatusAndMessage()
    {
        RequestResult result = new((RequestResult.OkCode, "tuple msg"));
        Assert.AreEqual(RequestResult.OkCode, result.Code);
        Assert.AreEqual("tuple msg", result.Message);
    }
}
