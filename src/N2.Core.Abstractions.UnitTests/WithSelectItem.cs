using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithSelectItem
{
    // -------------------------------------------------------------------------
    // SelectItem (base)
    // -------------------------------------------------------------------------

    [TestMethod]
    public void BaseDefaultCtorSetsEmptyKeyNullGroupFalseDisabled()
    {
        SelectItem item = new();
        Assert.AreEqual(Guid.Empty, item.Key);
        Assert.IsNull(item.Group);
        Assert.IsFalse(item.Disabled);
    }

    // -------------------------------------------------------------------------
    // SelectItem<T>
    // -------------------------------------------------------------------------

    [TestMethod]
    public void GenericDefaultCtorSetsNullValue()
    {
        SelectItem<string> item = new();
        Assert.IsNull(item.Value);
        Assert.AreEqual(Guid.Empty, item.Key);
    }

    [TestMethod]
    public void ValueCtorSetsValue()
    {
        SelectItem<string> item = new("hello");
        Assert.AreEqual("hello", item.Value);
        Assert.AreEqual(Guid.Empty, item.Key);
    }

    [TestMethod]
    public void KeyValueCtorSetsBothKeyAndValue()
    {
        Guid key = Guid.NewGuid();
        SelectItem<int> item = new(key, 42);
        Assert.AreEqual(key, item.Key);
        Assert.AreEqual(42, item.Value);
    }
}
