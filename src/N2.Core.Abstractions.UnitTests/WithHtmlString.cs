using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Abstractions.UnitTests;

[TestClass]
public class WithHtmlString
{
    [TestMethod]
    public void DefaultCtorProducesEmptyOutput()
    {
        HtmlString html = new();
        Assert.AreEqual(string.Empty, html.ToHtmlString());
    }

    [TestMethod]
    public void StringCtorSetsRawData()
    {
        HtmlString html = new("hello");
        Assert.AreEqual("hello", html.RawData);
        Assert.AreEqual("hello", html.ToHtmlString());
    }

    [TestMethod]
    public void ToHtmlStringEncodesLessThan()
    {
        HtmlString html = new("<div>");
        Assert.IsTrue(html.ToHtmlString().Contains("&lt;", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ToHtmlStringEncodesGreaterThan()
    {
        HtmlString html = new("<div>");
        Assert.IsTrue(html.ToHtmlString().Contains("&gt;", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ToHtmlStringEncodesAmpersand()
    {
        HtmlString html = new("a&b");
        Assert.IsTrue(html.ToHtmlString().Contains("&amp;", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ToHtmlStringDoesNotModifyPlainText()
    {
        HtmlString html = new("hello world");
        Assert.AreEqual("hello world", html.ToHtmlString());
    }

    [TestMethod]
    public void RawDataCanBeSetAfterConstruction()
    {
        HtmlString html = new();
        html.RawData = "<b>bold</b>";
        Assert.IsTrue(html.ToHtmlString().Contains("&lt;", StringComparison.Ordinal));
    }
}
