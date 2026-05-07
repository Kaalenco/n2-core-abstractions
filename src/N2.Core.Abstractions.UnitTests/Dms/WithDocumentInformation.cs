using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core.Dms;

[TestClass]
public sealed class WithDocumentInformation
{
    [TestMethod]
    public void DefaultCtorSetsCreatedToUnixEpochUtc()
    {
        DocumentInformation info = new();
        Assert.AreEqual(1970, info.Created.Year);
        Assert.AreEqual(1, info.Created.Month);
        Assert.AreEqual(1, info.Created.Day);
        Assert.AreEqual(0, info.Created.Hour);
        Assert.AreEqual(0, info.Created.Minute);
        Assert.AreEqual(0, info.Created.Second);
        Assert.AreEqual(DateTimeKind.Utc, info.Created.Kind);
    }

    [TestMethod]
    public void DefaultCtorInitializesEmptyStringsAndCollections()
    {
        DocumentInformation info = new();
        Assert.AreEqual(string.Empty, info.ProcessName);
        Assert.AreEqual(string.Empty, info.DocumentIdentifier);
        Assert.AreEqual(string.Empty, info.Remarks);
        Assert.AreEqual(string.Empty, info.FileName);
        Assert.AreEqual(string.Empty, info.Extension);
        Assert.IsNotNull(info.Roles);
        Assert.AreEqual(0, info.Roles.Count);
        Assert.IsNotNull(info.Tags);
        Assert.AreEqual(0, info.Tags.Count);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        Guid publicId = Guid.NewGuid();
        Guid uploadedBy = Guid.NewGuid();
        DateTime created = new(2024, 3, 15, 12, 0, 0, DateTimeKind.Utc);
        DocumentInformation info = new()
        {
            PublicId = publicId,
            ProcessName = "Invoices",
            DcmiType = AttachmentType.Text,
            FileName = "invoice.pdf",
            Extension = "pdf",
            IsEnabled = true,
            Size = 2048,
            Created = created,
            UploadedBy = uploadedBy
        };

        Assert.AreEqual(publicId, info.PublicId);
        Assert.AreEqual("Invoices", info.ProcessName);
        Assert.AreEqual(AttachmentType.Text, info.DcmiType);
        Assert.AreEqual("invoice.pdf", info.FileName);
        Assert.AreEqual(created, info.Created);
        Assert.AreEqual(DateTimeKind.Utc, info.Created.Kind);
        Assert.IsTrue(info.IsEnabled);
        Assert.AreEqual(uploadedBy, info.UploadedBy);
    }
}
