using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Dms;

[TestClass]
public sealed class WithAttachmentType
{
    [TestMethod]
    public void UnspecifiedIsZero()
    {
        Assert.AreEqual(0, (int)AttachmentType.Unspecified);
    }

    [TestMethod]
    public void ValuesMatchExpectedIntegers()
    {
        Assert.AreEqual(1, (int)AttachmentType.Collection);
        Assert.AreEqual(2, (int)AttachmentType.Dataset);
        Assert.AreEqual(3, (int)AttachmentType.Event);
        Assert.AreEqual(4, (int)AttachmentType.Image);
        Assert.AreEqual(5, (int)AttachmentType.InteractiveResource);
        Assert.AreEqual(6, (int)AttachmentType.Service);
        Assert.AreEqual(7, (int)AttachmentType.Software);
        Assert.AreEqual(8, (int)AttachmentType.Sound);
        Assert.AreEqual(9, (int)AttachmentType.Text);
    }
}
