using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

[TestClass]
public class WithGuidResponse
{
    [TestMethod]
    public void AcceptSetsStatus200AndValue()
    {
        Guid id = Guid.NewGuid();
        GuidResponse response = GuidResponse.Accept(id);
        Assert.AreEqual(id, response.Value);
        Assert.AreEqual(ResponseStatus.Success, response.Status);
        Assert.IsTrue(response.Success);
    }

    [TestMethod]
    public void CreatedSetsStatus201AndValue()
    {
        Guid id = Guid.NewGuid();
        GuidResponse response = GuidResponse.Created(id);
        Assert.AreEqual(id, response.Value);
        Assert.AreEqual(ResponseStatus.Created, response.Status);
    }

    [TestMethod]
    public void FailSetsStatus500AndMessage()
    {
        Guid id = Guid.NewGuid();
        GuidResponse response = GuidResponse.Fail(id, "something went wrong");
        Assert.AreEqual(id, response.Value);
        Assert.AreEqual(ResponseStatus.ServerError, response.Status);
        Assert.AreEqual("something went wrong", response.Message);
        Assert.IsFalse(response.Success);
    }

    [TestMethod]
    public void NoChangesSetsStatus204()
    {
        Guid id = Guid.NewGuid();
        GuidResponse response = GuidResponse.NoChanges(id, "no changes");
        Assert.AreEqual(id, response.Value);
        Assert.AreEqual(ResponseStatus.NoContent, response.Status);
    }

    [TestMethod]
    public void EqualsReturnsTrueForSameValue()
    {
        Guid id = Guid.NewGuid();
        GuidResponse a = GuidResponse.Accept(id);
        GuidResponse b = GuidResponse.Accept(id);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void EqualsReturnsFalseForDifferentValue()
    {
        GuidResponse a = GuidResponse.Accept(Guid.NewGuid());
        GuidResponse b = GuidResponse.Accept(Guid.NewGuid());
        Assert.IsFalse(a.Equals(b));
    }

    [TestMethod]
    public void EqualsReturnsFalseForNull()
    {
        GuidResponse response = GuidResponse.Accept(Guid.NewGuid());
        Assert.IsFalse(response.Equals((ICommandResponse<Guid>?)null));
    }

    [TestMethod]
    public void DeconstructReturnsValueAndStatus()
    {
        Guid id = Guid.NewGuid();
        GuidResponse response = GuidResponse.Accept(id);
        var (value, status) = response;
        Assert.AreEqual(id, value);
        Assert.AreEqual(ResponseStatus.Success, status);
    }

    [TestMethod]
    public void GetHashCodeIsDeterministic()
    {
        Guid id = Guid.NewGuid();
        GuidResponse a = GuidResponse.Accept(id);
        GuidResponse b = GuidResponse.Accept(id);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
}
