using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Dms;

[TestClass]
public sealed class WithDocument
{
    [TestMethod]
    public void DefaultCtorSetsEmptyStringsAndFalseFlags()
    {
        Document doc = new();
        Assert.AreEqual(0, doc.Id);
        Assert.IsNull(doc.Size);
        Assert.AreEqual(string.Empty, doc.OriginalName);
        Assert.AreEqual(string.Empty, doc.ProcessName);
        Assert.AreEqual(string.Empty, doc.DocumentType);
        Assert.AreEqual(string.Empty, doc.Extension);
        Assert.AreEqual(string.Empty, doc.Location);
        Assert.AreEqual(string.Empty, doc.ExtensionGroup);
        Assert.AreEqual(string.Empty, doc.Remarks);
        Assert.IsFalse(doc.IsPrivate);
        Assert.IsFalse(doc.IsEnabled);
        Assert.IsFalse(doc.IsRemoved);
        Assert.IsNull(doc.Removed);
        Assert.AreEqual(AttachmentType.Unspecified, doc.DcmiType);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Guid createdBy = Guid.NewGuid();
        Guid publicId = Guid.NewGuid();
        Document doc = new()
        {
            Id = 42,
            Size = 1024,
            OriginalName = "report.pdf",
            ProcessName = "Invoices",
            DocumentType = "Invoice",
            Extension = "pdf",
            Location = "/docs/report.pdf",
            CreatedBy = createdBy,
            IsEnabled = true,
            PublicId = publicId,
            DcmiType = AttachmentType.Text,
            Roles = ["admin", "user"],
            Tags = ["report", "2024"]
        };

        Assert.AreEqual(42, doc.Id);
        Assert.AreEqual(1024, doc.Size);
        Assert.AreEqual("report.pdf", doc.OriginalName);
        Assert.AreEqual("Invoice", doc.DocumentType);
        Assert.AreEqual("pdf", doc.Extension);
        Assert.AreEqual(createdBy, doc.CreatedBy);
        Assert.IsTrue(doc.IsEnabled);
        Assert.AreEqual(publicId, doc.PublicId);
        Assert.AreEqual(AttachmentType.Text, doc.DcmiType);
        Assert.AreEqual(2, doc.Roles.Length);
        Assert.AreEqual(2, doc.Tags.Length);
    }
}
