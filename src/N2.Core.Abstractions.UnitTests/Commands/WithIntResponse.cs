using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.Commands;

[TestClass]
public sealed class WithIntResponse
{
    [TestMethod]
    public void ValueStatusCtorSetsValueAndStatus()
    {
        IntResponse response = new(42, (int)ResponseStatus.Success);
        Assert.AreEqual(42, response.Value);
        Assert.AreEqual(ResponseStatus.Success, response.Status);
        Assert.IsNull(response.Message);
    }

    [TestMethod]
    public void ValueStatusMessageCtorSetsAllThree()
    {
        IntResponse response = new(7, (int)ResponseStatus.NotFound, "item not found");
        Assert.AreEqual(7, response.Value);
        Assert.AreEqual(ResponseStatus.NotFound, response.Status);
        Assert.AreEqual("item not found", response.Message);
    }

    [TestMethod]
    public void Deconstruct_ReturnsValueAndStatus()
    {
        IntResponse response = new(99, (int)ResponseStatus.Created);
        var (value, status) = response;
        Assert.AreEqual(99, value);
        Assert.AreEqual(ResponseStatus.Created, status);
    }

    // -------------------------------------------------------------------------
    // Equals — compares Value only, Status is not considered
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Equals_SameValue_ReturnsTrue()
    {
        IntResponse a = new(10, (int)ResponseStatus.Success);
        IntResponse b = new(10, (int)ResponseStatus.Success);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void Equals_SameValueDifferentStatus_ReturnsTrue()
    {
        IntResponse a = new(10, (int)ResponseStatus.Success);
        IntResponse b = new(10, (int)ResponseStatus.NotFound);
        Assert.IsTrue(a.Equals(b));
    }

    [TestMethod]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        IntResponse a = new(1, (int)ResponseStatus.Success);
        IntResponse b = new(2, (int)ResponseStatus.Success);
        Assert.IsFalse(a.Equals(b));
    }

    [TestMethod]
    public void Equals_Null_ReturnsFalse()
    {
        IntResponse response = new(5, (int)ResponseStatus.Success);
        Assert.IsFalse(response.Equals((ICommandResponse<int>?)null));
    }

    // -------------------------------------------------------------------------
    // GetHashCode
    // -------------------------------------------------------------------------

    [TestMethod]
    public void GetHashCode_IsDeterministic()
    {
        IntResponse a = new(42, (int)ResponseStatus.Success);
        IntResponse b = new(42, (int)ResponseStatus.Success);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
}
