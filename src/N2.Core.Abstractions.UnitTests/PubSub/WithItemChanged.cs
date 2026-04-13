using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.PubSub;

namespace N2.Core.Abstractions.UnitTests.PubSub;

[TestClass]
public class WithItemChanged
{
    [TestMethod]
    public void ItemChangedEqualsShouldReturnTrueForEqualItems()
    {
        ItemChanged item1 = new( type: typeof(string), uuid: Guid.NewGuid(), dateTime: DateTime.Now );
        ItemChanged item2 = new( type: typeof(string), uuid: item1.Uuid, dateTime: item1.DateTime );
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);

        item1 = new( type: typeof(string), uuid: Guid.NewGuid(), dateTime: DateTime.Now );
        item2 = new( type: typeof(string), uuid: item1.Uuid, dateTime: DateTime.Now.AddMinutes(2) );
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);
    }

    [TestMethod]
    public void ItemChangedEqualsShouldReturnTrueForEqualItemsWithDifferentDates()
    {
        ItemChanged item1 = new( type: typeof(string), uuid: Guid.NewGuid(), dateTime: DateTime.Now );
        ItemChanged item2 = new( type: typeof(string), uuid: item1.Uuid, dateTime: DateTime.Now.AddMinutes(2) );
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item1 == item2);
    }

    [TestMethod]
    public void ItemChangedEqualsShouldReturnFalseForDifferentItems()
    {
        ItemChanged item1 = new( type: typeof(string), uuid: Guid.NewGuid());
        ItemChanged item2 = new( type: typeof(int), uuid: item1.Uuid );
        Assert.IsFalse(item1.Equals(item2));
        Assert.IsFalse(item1 == item2);

        item1 = new(type: typeof(string), uuid: Guid.NewGuid());
        item2 = new(type: typeof(string), uuid: Guid.NewGuid());
        Assert.IsFalse(item1.Equals(item2));
        Assert.IsFalse(item1 == item2);
    }
}
