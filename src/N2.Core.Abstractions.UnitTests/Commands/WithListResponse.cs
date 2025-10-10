using Microsoft.VisualStudio.TestTools.UnitTesting;

using N2.Core.Commands;

namespace N2.Core.Abstractions.UnitTests.Commands;
[TestClass]
public sealed class WithListResponse
{
    [TestMethod]
    public void ListResponseCanBeCrteatedFromList()
    {
        List<string> list = new()
        { "a", "b" };
        ListResponse<string> response = new(list);
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Value);
        Assert.AreEqual(2, response.Value.Count);
        Assert.AreEqual(ResponseStatus.Success, response.Status);
    }
}
