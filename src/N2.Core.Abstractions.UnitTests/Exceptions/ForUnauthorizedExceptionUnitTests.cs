using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Exceptions;

[TestClass]
public sealed class ForUnauthorizedExceptionUnitTests
{
    [TestMethod]
    public void DefaultCtorSetsUnauthorized()
    {
        UnauthorizedException ex = new();
        Assert.AreEqual(ResponseStatus.Unauthorized, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorPreservesMessage()
    {
        UnauthorizedException ex = new("access denied");
        Assert.AreEqual("access denied", ex.Message);
    }

    [TestMethod]
    public void StringInnerCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        UnauthorizedException ex = new("unauthorized access", inner);
        Assert.AreEqual("unauthorized access", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsThrowableAsN2CoreException()
    {
        Assert.Throws<UnauthorizedException>(() => throw new UnauthorizedException("forbidden"));
        Assert.IsInstanceOfType<N2CoreException>(new UnauthorizedException());
    }
}
