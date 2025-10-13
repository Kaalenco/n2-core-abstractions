using N2.Core.Identity;

namespace N2.Core.Dms;

/// <summary>
/// A data repository for documents.
/// </summary>
public interface IDocumentRepository
{
    Task<int> CompleteAsync(IUserContext userMatrix, CancellationToken token);

    bool DocumentExists(string fileIdentifier);

    void SaveDocument(Document file);

    Task<Document> FindDocumentAsync(Guid publicId, CancellationToken token);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted, CancellationToken token);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted, bool isPublic, bool isEnabled, CancellationToken token);

    Task<Document> FindDocumentAsync(Guid publicId, bool isDeleted, Guid createdBy, CancellationToken token);

    Task RemoveDocument(Guid publicId, CancellationToken token);

    Task Cleanup(CancellationToken token);

    IQueryable<Document> DocumentQuery { get; }
}