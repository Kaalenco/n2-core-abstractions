using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

/// <summary>
/// The command request tests.
/// </summary>
[TestClass]
public class CommandRequestTests
{
    /// <summary>
    /// A simple request object
    /// </summary>
    private sealed class TestCommandRequest : CommandRequest
    {
    }

    /// <summary>
    /// Validate serialization with empty fields
    /// </summary>
    [TestMethod]
    public void CommandRequestShouldSerialize()
    {
        TestCommandRequest item = new();
        string json = JsonSerializer.Serialize(item);
        Assert.IsNotNull(json);
        Assert.AreEqual("{}", json);
    }

    /// <summary>
    /// Validate serialization with empty fields
    /// </summary>
    [TestMethod]
    public void CommandRequestSerializationShouldIgnoreNullableFields()
    {
        TestCommandRequest item = new();
        string json = JsonSerializer.Serialize(item);
        Assert.IsNotNull(json);
        Assert.AreEqual("{}", json);
    }

    /// <summary>
    /// Validate deserialization.
    /// </summary>
    /// <param name="source">
    /// </param>
    /// <param name="expectHandle">
    /// </param>
    /// <param name="expectValidate">
    /// </param>
    [DataTestMethod]
    [DataRow("{\"DoNotProcess\":\"F\"}", null, false)]
    [DataRow("{\"DoNotProcess\":\"T\"}", null, true)]
    [DataRow("{\"Handle\":\"\", \"DoNotProcess\":\"T\"}", "", true)]
    [DataRow("{\"Handle\":\"Hello command\", \"DoNotProcess\":\"T\"}", "Hello command", true)]
    public void CommandRequestShouldDeSerialize(
        string source,
        string expectHandle,
        bool expectValidate)
    {
        TestCommandRequest? item = JsonSerializer.Deserialize<TestCommandRequest>(source);

        Assert.IsNotNull(item);
        Assert.AreEqual(expectHandle, item.Handle);
        Assert.AreEqual(expectValidate, item.DoNotProcess == 'T');
    }
}