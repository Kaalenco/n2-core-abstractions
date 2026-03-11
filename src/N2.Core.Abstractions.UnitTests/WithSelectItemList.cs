using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Abstractions.UnitTests;

[TestClass]
public class WithSelectItemList
{
    [TestMethod]
    public void DefaultCtorCreatesEmptyCollection()
    {
        SelectItemList<string> list = new();
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void TupleSequenceCtorPopulatesItems()
    {
        Guid key1 = Guid.NewGuid();
        Guid key2 = Guid.NewGuid();
        var items = new[] { (key1, "alpha"), (key2, "beta") };

        SelectItemList<string> list = new(items);

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual(key1, list[0].Key);
        Assert.AreEqual("alpha", list[0].Value);
        Assert.AreEqual(key2, list[1].Key);
        Assert.AreEqual("beta", list[1].Value);
    }

    [TestMethod]
    public void NullSequenceCtorProducesEmptyCollection()
    {
        SelectItemList<string> list = new(null!);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void EmptySequenceCtorProducesEmptyCollection()
    {
        SelectItemList<string> list = new(Array.Empty<(Guid, string)>());
        Assert.AreEqual(0, list.Count);
    }
}
