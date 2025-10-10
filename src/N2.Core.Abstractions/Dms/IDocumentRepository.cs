using N2.Core.Identity;

namespace N2.Core.Dms;

/// <summary>
/// A data repository for documents.
/// </summary>
public interface IDocumentRepository
{
    Task<int> CompleteAsync(IUserContext userMatrix);

    bool DocumentExists(string fileIdentifier);

    void SaveDocument(Document file);

    Task<Document> FindDocumentAsync(Guid publicId);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted, bool isPublic, bool isEnabled);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted, Guid createdBy);

    IAsyncEnumerable<Document> DocumentQuery { get; }
}