using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithErrorCode
{
    // -------------------------------------------------------------------------
    // ErrorCode enum
    // -------------------------------------------------------------------------

    [TestMethod]
    public void NoneIsZero()
    {
        Assert.AreEqual(0, (int)ErrorCode.None);
    }

    [TestMethod]
    public void ValuesAreOrdered()
    {
        Assert.IsTrue((int)ErrorCode.ValueNullOrEmpty > (int)ErrorCode.None);
        Assert.IsTrue((int)ErrorCode.StartLowerThenEnd > (int)ErrorCode.ValueNullOrEmpty);
        Assert.IsTrue((int)ErrorCode.MinimumValue > (int)ErrorCode.StartLowerThenEnd);
        Assert.IsTrue((int)ErrorCode.StringLenght > (int)ErrorCode.MinimumValue);
    }

    // -------------------------------------------------------------------------
    // ErrorCodeDescription
    // -------------------------------------------------------------------------

    [TestMethod]
    public void ErrorCodeDescription_DefaultCtorSetsEmptyStrings()
    {
        ErrorCodeDescription desc = new();
        Assert.AreEqual(string.Empty, desc.Code);
        Assert.AreEqual(string.Empty, desc.Description);
    }

    [TestMethod]
    public void ErrorCodeDescription_PropertiesCanBeSet()
    {
        ErrorCodeDescription desc = new() { Code = "ERR001", Description = "Value is null or empty" };
        Assert.AreEqual("ERR001", desc.Code);
        Assert.AreEqual("Value is null or empty", desc.Description);
    }
}
