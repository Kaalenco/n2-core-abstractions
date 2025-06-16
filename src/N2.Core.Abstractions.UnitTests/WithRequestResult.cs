using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Abstractions.UnitTests;

[TestClass]
public sealed class WithRequestResult
{
    [TestMethod]
    public void WithHandleShouldReturnNewRequestResult()
    {
        Guid newHandle = Guid.NewGuid();
        RequestResult original = new(RequestResult.OkCode, "Original message");
        RequestResult updated = RequestResultExtensions.WithHandle(original, newHandle);
        Assert.AreEqual(original.Status, updated.Status);
        Assert.AreEqual(original.MessageOrDefault(), updated.MessageOrDefault());
        Assert.AreEqual(newHandle, updated.Handle);
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
