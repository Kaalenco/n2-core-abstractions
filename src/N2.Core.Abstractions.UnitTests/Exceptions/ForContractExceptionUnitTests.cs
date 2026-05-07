using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Exceptions;

[TestClass]
public sealed class ForContractExceptionUnitTests
{
    [TestMethod]
    public void DefaultCtorSetsNotAcceptedStatus()
    {
        ContractException ex = new();
        Assert.AreEqual(ResponseStatus.NotAccepted, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorSetsNotAcceptableStatus()
    {
        ContractException ex = new("contract violated");
        Assert.AreEqual("contract violated", ex.Message);
        Assert.AreEqual(ResponseStatus.NotAcceptable, ex.ResponseStatus);
    }

    [TestMethod]
    public void StringInnerCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        ContractException ex = new("contract broken", inner);
        Assert.AreEqual("contract broken", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
        Assert.AreEqual(ResponseStatus.NotAcceptable, ex.ResponseStatus);
    }

    [TestMethod]
    public void IsThrowableAsN2CoreException()
    {
        Assert.Throws<ContractException>(() => throw new ContractException("bad contract"));
        Assert.IsInstanceOfType<N2CoreException>(new ContractException());
    }
}
