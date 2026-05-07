using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Identity;

[TestClass]
public sealed class WithUserSelectItem
{
    [TestMethod]
    public void DefaultCtorSetsGuidEmptyAndNullStrings()
    {
        UserSelectItem item = new();
        Assert.AreEqual(Guid.Empty, item.Key);
        Assert.IsNull(item.DisplayName);
        Assert.IsNull(item.Email);
        Assert.IsNull(item.ImagePath);
        Assert.IsFalse(item.RemoveThisRecord);
    }

    [TestMethod]
    public void ToHtmlString_ReturnsHtmlEncodedDisplayName()
    {
        UserSelectItem item = new() { DisplayName = "<script>alert('xss')</script>" };
        string html = item.ToHtmlString();
        Assert.IsFalse(html.Contains('<'));
        Assert.IsFalse(html.Contains('>'));
    }

    [TestMethod]
    public void ToHtmlString_FallsBackToEmail_WhenDisplayNameIsNull()
    {
        UserSelectItem item = new() { Email = "user@example.com" };
        Assert.AreEqual("user@example.com", item.ToHtmlString());
    }

    [TestMethod]
    public void ToHtmlString_ReturnsEmptyString_WhenBothAreNull()
    {
        UserSelectItem item = new();
        Assert.AreEqual(string.Empty, item.ToHtmlString());
    }

    [TestMethod]
    public void Clone_CreatesCopyWithSameValues()
    {
        Guid key = Guid.NewGuid();
        UserSelectItem original = new()
        {
            Key = key,
            DisplayName = "Alice",
            Email = "alice@example.com",
            ImagePath = "/img/alice.png"
        };

        UserSelectItem clone = (UserSelectItem)original.Clone();

        Assert.AreEqual(key, clone.Key);
        Assert.AreEqual("Alice", clone.DisplayName);
        Assert.AreEqual("alice@example.com", clone.Email);
        Assert.AreEqual("/img/alice.png", clone.ImagePath);
        Assert.AreNotSame(original, clone);
    }
}
