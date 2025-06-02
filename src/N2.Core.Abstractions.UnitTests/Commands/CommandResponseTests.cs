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

        public TestCommandResponse(ResponseStatus status, string message, string handle)
            : base(status, message, handle)
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
        TestCommandResponse response = new(ResponseStatus.Success, "Test message", "test-handle");
        ICommandResponse item = response.CreateNew(ResponseStatus.NoContent, "No content message", "new-handle");

        Assert.IsNotNull(item);
        Assert.IsInstanceOfType<TestCommandResponse>(item);
        Assert.AreEqual(ResponseStatus.NoContent, item.Status);
        Assert.AreNotEqual(ResponseStatus.NoContent, response.Status);
    }
}
