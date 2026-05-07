using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Commands;

[TestClass]
public sealed class WithPagedRequest
{
    // -------------------------------------------------------------------------
    // Default values
    // -------------------------------------------------------------------------

    [TestMethod]
    public void DefaultCtorInitializesPageAndItemsPerPageToZero()
    {
        PagedRequest<object> r = new();
        Assert.AreEqual(0, r.Page);
        Assert.AreEqual(0, r.ItemsPerPage);
        Assert.IsNull(r.Filter);
    }

    [TestMethod]
    public void SettingPropertiesIsReflected()
    {
        PagedRequest<object> r = new() { Page = 3, ItemsPerPage = 20, Filter = "active" };
        Assert.AreEqual(3, r.Page);
        Assert.AreEqual(20, r.ItemsPerPage);
        Assert.AreEqual("active", r.Filter);
    }

    // -------------------------------------------------------------------------
    // JSON serialization
    // -------------------------------------------------------------------------

    [TestMethod]
    public void Serialization_FilterExcludedWhenNull()
    {
        PagedRequest<object> r = new() { Page = 1, ItemsPerPage = 10 };
        string json = JsonSerializer.Serialize(r);
        Assert.IsFalse(json.Contains("Filter", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Serialization_FilterIncludedWhenSet()
    {
        PagedRequest<object> r = new() { Page = 1, ItemsPerPage = 10, Filter = "active" };
        string json = JsonSerializer.Serialize(r);
        Assert.IsTrue(json.Contains("\"Filter\"", StringComparison.Ordinal));
        Assert.IsTrue(json.Contains("active", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Serialization_PageAndItemsPerPageAlwaysPresent()
    {
        PagedRequest<object> r = new() { Page = 5, ItemsPerPage = 25 };
        string json = JsonSerializer.Serialize(r);
        Assert.IsTrue(json.Contains("\"Page\":5", StringComparison.Ordinal));
        Assert.IsTrue(json.Contains("\"ItemsPerPage\":25", StringComparison.Ordinal));
    }
}
