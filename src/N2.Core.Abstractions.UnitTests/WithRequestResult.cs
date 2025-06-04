using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Abstractions.UnitTests;

[TestClass]
public sealed class WithRequestResult
{
    [TestMethod]
    public void WithHandleShouldReturnNewRequestResult()
    {
        RequestResult original = new(RequestResult.OkCode, "Original message");
        RequestResult updated = RequestResultExtensions.WithHandle(original, "new-handle");
        Assert.AreEqual(original.Status, updated.Status);
        Assert.AreEqual(original.MessageOrDefault(), updated.MessageOrDefault());
        Assert.AreEqual("new-handle", updated.Handle);
    }
    [TestMethod]
    public void MessageOrDefaultShouldReturnDefaultMessageWhenEmpty()
    {
        RequestResult result = new(RequestResult.OkCode, string.Empty);
        string message = result.MessageOrDefault("Default message");
        Assert.AreEqual("Default message", message);
    }

    [TestMethod]
    public void MessageOrDefaultShouldReturnMessageWhenNotEmpty()
    {
        RequestResult result = new(RequestResult.OkCode, "Custom message");
        string message = result.MessageOrDefault("Default message");
        Assert.AreEqual("Custom message", message);
    }
}
