using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.PubSub;

namespace N2.Core.UnitTests.PubSub;

[TestClass]
public class WithItemChanged
{
    [TestMethod]
    public void ItemChangedEqualsShouldReturnTrueForEqualItems()
    {
        ItemChanged item1 = new() { Type = typeof(string), Uuid = Guid.NewGuid(), DateTime = DateTime.Now };
        ItemChanged item2 = new() { Type = typeof(string), Uuid = item1.Uuid, DateTime = item1.DateTime };
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);

        item1 = new() { Type = typeof(string), Uuid = Guid.NewGuid(), DateTime = DateTime.Now };
        item2 = new() { Type = typeof(string), Uuid = item1.Uuid, DateTime = DateTime.Now.AddMinutes(2) };
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);
    }

    [TestMethod]
    public void ItemChangedEqualsShouldReturnTrueForEqualItemsWithDifferentDates()
    {
        ItemChanged item1 = new() { Type = typeof(string), Uuid = Guid.NewGuid(), DateTime = DateTime.Now };
        ItemChanged item2 = new() { Type = typeof(string), Uuid = item1.Uuid, DateTime = DateTime.Now.AddMinutes(2) };
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);
    }

    [TestMethod]
    public void ItemChangedEqualsShouldReturnFalseForDifferentItems()
    {
        ItemChanged item1 = new() { Type = typeof(string), Uuid = Guid.NewGuid() };
        ItemChanged item2 = new() { Type = typeof(int), Uuid = item1.Uuid };
        Assert.IsFalse(item1.Equals(item2));
        Assert.IsFalse(item1 == item2);

        item1 = new() { Type = typeof(string), Uuid = Guid.NewGuid() };
        item2 = new() { Type = typeof(string), Uuid = Guid.NewGuid() };
        Assert.IsFalse(item1.Equals(item2));
        Assert.IsFalse(item1 == item2);
    }
}
