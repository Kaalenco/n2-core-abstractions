using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Exceptions;

[TestClass]
public sealed class ForElementNotFoundExceptionUnitTests
{
    [TestMethod]
    public void DefaultCtorSetsNotFound()
    {
        ElementNotFoundException ex = new();
        Assert.AreEqual(ResponseStatus.NotFound, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorPreservesMessage()
    {
        ElementNotFoundException ex = new("item not found");
        Assert.AreEqual("item not found", ex.Message);
    }

    [TestMethod]
    public void StringInnerCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        ElementNotFoundException ex = new("missing resource", inner);
        Assert.AreEqual("missing resource", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsThrowableAsN2CoreException()
    {
        Assert.Throws<ElementNotFoundException>(() => throw new ElementNotFoundException("not found"));
        Assert.IsInstanceOfType<N2CoreException>(new ElementNotFoundException());
    }
}
