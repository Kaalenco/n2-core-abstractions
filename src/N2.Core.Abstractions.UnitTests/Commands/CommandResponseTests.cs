using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

[TestClass]
public class CommandResponseTests
{
    /// <summary>
    /// A simple request object
    /// </summary>
    private sealed class TestCommandResponse : CommandResponse
    {
        public TestCommandResponse()
        {

        }

        public TestCommandResponse(ResponseStatus status, string message, Guid handle)
            : base(status, message, handle)
        {
        }

        public TestCommandResponse(ResponseStatus status, string message)
            : base(status, message)
        {
        }
    }

    [TestMethod]
    public void CommandResponseShouldSerialize()
    {
        TestCommandResponse item = new();
        string json = JsonSerializer.Serialize(item);
        Assert.IsNotNull(json);
        Assert.AreEqual("{\"Status\":200,\"Success\":true}", json);
    }

    [TestMethod]
    public void CommandResponseCanInitializeNew()
    {
        Guid testHandle = Guid.NewGuid();
        Guid newHandle = Guid.NewGuid();
        TestCommandResponse response = new(ResponseStatus.Success, "Test message", testHandle);
        ICommandResponse item = response.CreateNew(ResponseStatus.NoContent, "No content message", newHandle);

        Assert.IsNotNull(item);
        Assert.IsInstanceOfType<TestCommandResponse>(item);
        Assert.AreEqual(ResponseStatus.NoContent, item.Status);
        Assert.AreNotEqual(ResponseStatus.NoContent, response.Status);
    }

    [TestMethod]
    public void CommandResponseIsClonable()
    {
        Guid testHandle = Guid.NewGuid();
        TestCommandResponse response = new(ResponseStatus.Success, "Test message", testHandle);
        ICommandResponse clone = (ICommandResponse)response.Clone();
        Assert.IsNotNull(clone);
        Assert.IsInstanceOfType<TestCommandResponse>(clone);
        Assert.AreEqual(response.Status, clone.Status);
        Assert.AreEqual(response.Message, clone.Message);
        Assert.AreEqual(response.Handle, clone.Handle);
        Assert.AreEqual(response.ExecutionTime, clone.ExecutionTime);
    }

    [TestMethod]
    public void SuccessIsTrueForStatusAtOrBelow300()
    {
        TestCommandResponse ok = new(ResponseStatus.Success, "ok", Guid.Empty);
        Assert.IsTrue(ok.Success);

        // NotAccepted = 300, which is <= 300 so Success is still true
        TestCommandResponse notAccepted = new(ResponseStatus.NotAccepted, "not accepted");
        Assert.IsTrue(notAccepted.Success);
    }

    [TestMethod]
    public void SuccessIsFalseForStatusAbove300()
    {
        TestCommandResponse badRequest = new(ResponseStatus.BadRequest, "bad");
        Assert.IsFalse(badRequest.Success);

        TestCommandResponse serverError = new(ResponseStatus.ServerError, "error");
        Assert.IsFalse(serverError.Success);
    }

    [TestMethod]
    public void WithHandleSetsHandleAndReturnsSelf()
    {
        Guid handle = Guid.NewGuid();
        TestCommandResponse response = new();
        CommandResponse result = response.WithHandle(handle);
        Assert.AreSame(response, result);
        Assert.AreEqual(handle, response.Handle?.Value);
    }

    [TestMethod]
    public void WithExecutionTimeSetsTimeAndReturnsSelf()
    {
        TestCommandResponse response = new();
        CommandResponse result = response.WithExecutionTime(42L);
        Assert.AreSame(response, result);
        Assert.AreEqual(42L, response.ExecutionTime);
    }

    [TestMethod]
    public void ToStringFormatsStatusAndMessage()
    {
        TestCommandResponse response = new(ResponseStatus.Success, "all good");
        string text = response.ToString();
        Assert.IsTrue(text.Contains("Success", StringComparison.Ordinal));
        Assert.IsTrue(text.Contains("all good", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ImplicitIntConversionReturnsStatusCode()
    {
        TestCommandResponse response = new(ResponseStatus.Success, "ok", Guid.Empty);
        int code = response;
        Assert.AreEqual((int)ResponseStatus.Success, code);
    }

    [TestMethod]
    public void ToIntReturnsStatusCode()
    {
        TestCommandResponse response = new(ResponseStatus.Success, "ok", Guid.Empty);
        Assert.AreEqual((int)ResponseStatus.Success, response.ToInt());
    }

    [TestMethod]
    public void EqualsReturnsTrueForSameStatusAndHandle()
    {
        Guid handle = Guid.NewGuid();
        TestCommandResponse a = new(ResponseStatus.Success, "msg", handle);
        TestCommandResponse b = new(ResponseStatus.Success, "different msg", handle);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void EqualsReturnsFalseForDifferentStatus()
    {
        Guid handle = Guid.NewGuid();
        TestCommandResponse a = new(ResponseStatus.Success, "msg", handle);
        TestCommandResponse b = new(ResponseStatus.NotFound, "msg", handle);
        Assert.IsFalse(a.Equals(b));
    }

    [TestMethod]
    public void EqualsReturnsFalseForNull()
    {
        TestCommandResponse response = new();
        Assert.IsFalse(response.Equals(null));
    }
}
