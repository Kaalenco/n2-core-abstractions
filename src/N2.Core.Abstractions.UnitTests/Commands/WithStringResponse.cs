using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithStringResponse
{
    // -------------------------------------------------------------------------
    // Constructors
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Constructor_ValueAndStatus_SetsProperties()
    {
        StringResponse r = new("hello", 200);
        Assert.AreEqual("hello", r.Value);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
        Assert.IsNull(r.Message);
    }

    [TestMethod]
    public void Constructor_ValueStatusAndMessage_SetsAllProperties()
    {
        StringResponse r = new("body text", 404, "resource missing");
        Assert.AreEqual("body text", r.Value);
        Assert.AreEqual(ResponseStatus.NotFound, r.Status);
        Assert.AreEqual("resource missing", r.Message);
    }

    // -------------------------------------------------------------------------
    // Factory methods
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Accept_ReturnsStatus200WithValue()
    {
        StringResponse r = StringResponse.Accept("result");
        Assert.AreEqual("result", r.Value);
        Assert.AreEqual(ResponseStatus.Success, r.Status);
    }

    [TestMethod]
    public void Fail_ReturnsStatus500WithValueAndMessage()
    {
        StringResponse r = StringResponse.Fail("output", "something failed");
        Assert.AreEqual("output", r.Value);
        Assert.AreEqual(ResponseStatus.ServerError, r.Status);
        Assert.AreEqual("something failed", r.Message);
    }

    [TestMethod]
    public void NoContent_ReturnsStatus204WithValue()
    {
        StringResponse r = StringResponse.NoContent("empty");
        Assert.AreEqual("empty", r.Value);
        Assert.AreEqual(ResponseStatus.NoContent, r.Status);
    }

    [TestMethod]
    public void NoChange_ReturnsStatus203WithValue()
    {
        StringResponse r = StringResponse.NoChange("same");
        Assert.AreEqual("same", r.Value);
        Assert.AreEqual(ResponseStatus.Information, r.Status);
    }

    // -------------------------------------------------------------------------
    // Equals, GetHashCode, ToString, Deconstruct
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Equals_SameValue_ReturnsTrue()
    {
        StringResponse a = new("hello", 200);
        StringResponse b = new("hello", 404);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        StringResponse a = new("hello", 200);
        StringResponse b = new("world", 200);
        Assert.IsFalse(a.Equals(b));
    }

    [TestMethod]
    public void Equals_Null_ReturnsFalse()
    {
        StringResponse r = new("hello", 200);
        Assert.IsFalse(r.Equals(null));
    }

    [TestMethod]
    public void ToString_ReturnsValue()
    {
        StringResponse r = new("my-value", 200);
        Assert.AreEqual("my-value", r.ToString());
    }

    [TestMethod]
    public void Deconstruct_ReturnsValueAndStatus()
    {
        StringResponse r = new("data", 201);
        var (value, status) = r;
        Assert.AreEqual("data", value);
        Assert.AreEqual(ResponseStatus.Created, status);
    }
}
