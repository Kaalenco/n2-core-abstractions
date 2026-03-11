using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;
using N2.Core.Exceptions;

namespace N2.Core.Abstractions.UnitTests.Exceptions;

[TestClass]
public class WithN2CoreExceptionTests
{
    [TestMethod]
    public void DefaultCtorSetsNotAcceptedStatusAndMessage()
    {
        N2CoreException ex = new();
        Assert.AreEqual(ResponseStatus.NotAccepted, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorPreservesMessage()
    {
        N2CoreException ex = new("something failed");
        Assert.AreEqual("something failed", ex.Message);
        Assert.AreEqual(ResponseStatus.NotAccepted, ex.ResponseStatus);
    }

    [TestMethod]
    public void StatusMessageCtorSetsBothFields()
    {
        N2CoreException ex = new(ResponseStatus.NotFound, "resource missing");
        Assert.AreEqual(ResponseStatus.NotFound, ex.ResponseStatus);
        Assert.AreEqual("resource missing", ex.Message);
    }

    [TestMethod]
    public void StatusCtorUsesToMessageAsExceptionMessage()
    {
        N2CoreException ex = new(ResponseStatus.Unauthorized);
        Assert.AreEqual(ResponseStatus.Unauthorized, ex.ResponseStatus);
        Assert.AreEqual(ResponseStatus.Unauthorized.ToMessage(), ex.Message);
    }

    [TestMethod]
    public void InnerExceptionCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        N2CoreException ex = new("outer", inner);
        Assert.AreEqual("outer", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
        Assert.AreEqual(ResponseStatus.NotAccepted, ex.ResponseStatus);
    }

    [TestMethod]
    public void StatusMessageInnerExceptionCtorSetsAllFields()
    {
        InvalidOperationException inner = new("inner");
        N2CoreException ex = new(ResponseStatus.Forbidden, "forbidden", inner);
        Assert.AreEqual(ResponseStatus.Forbidden, ex.ResponseStatus);
        Assert.AreEqual("forbidden", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
    }

    [TestMethod]
    public void UseStatusUpdatesResponseStatusAndReturnsThis()
    {
        N2CoreException ex = new("error");
        N2CoreException result = ex.UseStatus(ResponseStatus.NotFound);
        Assert.AreSame(ex, result);
        Assert.AreEqual(ResponseStatus.NotFound, ex.ResponseStatus);
    }

    [TestMethod]
    public void IsThrowableAsException()
    {
        Assert.Throws<N2CoreException>(() => throw new N2CoreException("test"));
    }
}
