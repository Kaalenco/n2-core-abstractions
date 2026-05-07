using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithPagingInfo
{
    [TestMethod]
    public void DefaultCtorSetsExpectedDefaults()
    {
        PagingInfo info = new();
        Assert.AreEqual(string.Empty, info.Q);
        Assert.AreEqual(string.Empty, info.Sort);
        Assert.IsFalse(info.SortDesc);
        Assert.AreEqual(1, info.Page);
        Assert.AreEqual(10, info.PageSize);
        Assert.AreEqual(0, info.TotalItems);
        Assert.AreEqual(0, info.TotalPages);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        PagingInfo info = new()
        {
            Q = "search term",
            Sort = "Name",
            SortDesc = true,
            Page = 3,
            PageSize = 25,
            TotalItems = 100,
            TotalPages = 4
        };

        Assert.AreEqual("search term", info.Q);
        Assert.AreEqual("Name", info.Sort);
        Assert.IsTrue(info.SortDesc);
        Assert.AreEqual(3, info.Page);
        Assert.AreEqual(25, info.PageSize);
        Assert.AreEqual(100, info.TotalItems);
        Assert.AreEqual(4, info.TotalPages);
    }
}
