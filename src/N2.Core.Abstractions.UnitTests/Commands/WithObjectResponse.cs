using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithObjectResponse
{
    private sealed class Payload
    {
        public string Name { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------------------
    // Constructors
    // -------------------------------------------------------------------------

    [TestMethod]
    public void DefaultCtorSetsServerErrorStatusAndNewValue()
    {
        ObjectResponse<Payload> r = new();
        Assert.AreEqual(ResponseStatus.ServerError, r.Status);
        Assert.IsNotNull(r.Value);
    }

    [TestMethod]
    public void ValueStatusCtorSetsProperties()
    {
        Payload payload = new() { Name = "test" };
        ObjectResponse<Payload> r = new(payload, 200);
        Assert.AreSame(payload, r.Value);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
    }

    [TestMethod]
    public void ValueStatusMessageCtorSetsAllProperties()
    {
        Payload payload = new() { Name = "test" };
        ObjectResponse<Payload> r = new(payload, 404, "not found");
        Assert.AreSame(payload, r.Value);
        Assert.AreEqual(ResponseStatus.NotFound, r.Status);
        Assert.AreEqual("not found", r.Message);
    }

    // -------------------------------------------------------------------------
    // Factory methods
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Fail_ReturnsNewInstanceWithServerErrorAndMessage()
    {
        ObjectResponse<Payload> r = new();
        ObjectResponse<Payload> failed = r.Fail("something went wrong");
        Assert.AreEqual(ResponseStatus.ServerError, failed.Status);
        Assert.AreEqual("something went wrong", failed.Message);
        Assert.IsNotNull(failed.Value);
    }

    [TestMethod]
    public void Accept_ReturnsNewInstanceWithSuccessAndValue()
    {
        ObjectResponse<Payload> r = new();
        Payload payload = new() { Name = "accepted" };
        ObjectResponse<Payload> accepted = r.Accept(payload);
        Assert.AreEqual(ResponseStatus.Success, accepted.Status);
        Assert.AreSame(payload, accepted.Value);
    }

    // -------------------------------------------------------------------------
    // Equals, Deconstruct
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Equals_SameStatus_ReturnsTrue()
    {
        ObjectResponse<Payload> a = new(new Payload(), 200);
        ObjectResponse<Payload> b = new(new Payload(), 200);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void Equals_DifferentStatus_ReturnsFalse()
    {
        ObjectResponse<Payload> a = new(new Payload(), 200);
        ObjectResponse<Payload> b = new(new Payload(), 404);
        Assert.IsFalse(a.Equals(b));
    }

    [TestMethod]
    public void Equals_Null_ReturnsFalse()
    {
        ObjectResponse<Payload> r = new();
        Assert.IsFalse(r.Equals((ICommandResponse<Payload>?)null));
    }

    [TestMethod]
    public void Deconstruct_ReturnsValueAndStatus()
    {
        Payload payload = new() { Name = "deconstructed" };
        ObjectResponse<Payload> r = new(payload, 200);
        var (value, status) = r;
        Assert.AreSame(payload, value);
        Assert.AreEqual(ResponseStatus.Success, status);
    }
}
