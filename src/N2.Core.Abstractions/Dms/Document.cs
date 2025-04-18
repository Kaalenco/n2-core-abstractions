namespace N2.Core.Dms;

/// <summary>
/// The document information as stored in the document repository (DataModel).
/// </summary>
public class Document
{
    /// <summary>
    /// The unique identifier of the document, for internal use only.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The size of the document in bytes.
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// The original name of the document, as it was added to the document store.
    /// </summary>
    public string OriginalName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the process for which the document is intended.
    /// This could be the process that created the document or the process
    /// that handles the document.
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// The type specification within the process, for instance "Invoice" or "Contract".
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// The file extension of the document, without the separator.
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// The location or path where the document content is stored.
    /// This can be a file path, a URL, a database reference or a reference to a physical location.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the user or process that created the document.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// The date and time the document was created, in UTC.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Indication that this document and its metadata is privately owned by the user that created it.
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Indication that the document content is enabled and can be accessed.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Indication that the document is removed and should not be accessed.
    /// </summary>
    public bool IsRemoved { get; set; }

    /// <summary>
    /// The date and time the document was removed, in UTC.
    /// </summary>
    public DateTime? Removed { get; set; }

    /// <summary>
    /// The unique identifier for the document, for public use.
    /// </summary>
    public Guid PublicId { get; set; }

    /// <summary>
    /// The extensiongroup contains the mimetype of the document.
    /// </summary>
    public string ExtensionGroup { get; set; } = string.Empty;

    /// <summary>
    /// Optional remarks about the document.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// List of roles that can access this document.
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// Semicolon (;) separated list of tags that are associated with this document.
    /// </summary>
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// The Dcmi type of the document explains the intent of the document.
    /// For instance, a document with a type of "Image" is intended as
    /// a visual representation, even if it is a word file.
    /// </summary>
    public AttachmentType DcmiType { get; set; }
}
