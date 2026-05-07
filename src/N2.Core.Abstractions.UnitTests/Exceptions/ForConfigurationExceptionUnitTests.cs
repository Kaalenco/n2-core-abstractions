using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Exceptions;

[TestClass]
public sealed class ForConfigurationExceptionUnitTests
{
    [TestMethod]
    public void DefaultCtorSetsPreconditionFailed()
    {
        ConfigurationException ex = new();
        Assert.AreEqual(ResponseStatus.PreconditionFailed, ex.ResponseStatus);
        Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
    }

    [TestMethod]
    public void StringCtorPreservesMessage()
    {
        ConfigurationException ex = new("config missing");
        Assert.AreEqual("config missing", ex.Message);
    }

    [TestMethod]
    public void StringInnerCtorChainsCorrectly()
    {
        InvalidOperationException inner = new("inner");
        ConfigurationException ex = new("config problem", inner);
        Assert.AreEqual("config problem", ex.Message);
        Assert.AreEqual(inner, ex.InnerException);
    }

    [TestMethod]
    public void IsThrowableAsN2CoreException()
    {
        Assert.Throws<ConfigurationException>(() => throw new ConfigurationException("bad"));
        Assert.IsInstanceOfType<N2CoreException>(new ConfigurationException());
    }
}
