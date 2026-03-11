
using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;

/// <summary>
/// The paged response tests.
/// </summary>
[TestClass()]
public class PagedResponseTests
{
    private static readonly string[] items = new string[] { "a", "b", "c", "d", "e" };

    /// <summary>
    /// Pageds the response should initialize.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <param name="page">The page.</param>
    /// <param name="ipp">The ipp.</param>
    /// <param name="expectPages">The expected pages.</param>
    /// <param name="expectCount">The expected count.</param>
    /// <param name="expectPage">The expected page.</param>
    /// <param name="expectIpp">The expected ipp.</param>
    [TestMethod()]
    [DataRow(21, 1, 10, 3, 21, 1, 10)]
    [DataRow(21, -1, 10, 3, 21, 1, 10)]
    [DataRow(21, -1, 0, 1, 21, 1, 25)]
    public void PagedResponseShouldInitialize(int count, int page, int ipp, int expectPages, int expectCount, int expectPage, int expectIpp)
    {
        PagedResponse<string> item = new(count, page, ipp, items);
        Assert.IsNotNull(item);
        Assert.AreEqual(expectCount, item.TotalItemCount);
        Assert.AreEqual(expectPages, item.TotalPages);
        Assert.AreEqual(expectPage, item.Page);
        Assert.AreEqual(expectIpp, item.ItemsPerPage);


    }
}