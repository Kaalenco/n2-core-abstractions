using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Exceptions;

[TestClass]
public sealed class ForOperationExceptionUnitTests
{
    [TestMethod]
    public void DefaultCtorSetsMethodNotAllowed()
    {
        OperationException ex = new();
        Assert.AreEqual(ResponseStatus.MethodNotAllowed, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorPreservesMessage()
    {
        OperationException ex = new("operation not permitted");
        Assert.AreEqual("operation not permitted", ex.Message);
    }

    [TestMethod]
    public void StringInnerCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        OperationException ex = new("invalid operation", inner);
        Assert.AreEqual("invalid operation", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsThrowableAsN2CoreException()
    {
        Assert.Throws<OperationException>(() => throw new OperationException("not allowed"));
        Assert.IsInstanceOfType<N2CoreException>(new OperationException());
    }
}
