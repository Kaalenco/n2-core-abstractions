using N2.Core.Identity;

namespace N2.Core.Dms;

/// <summary>
/// A data repository for documents.
/// </summary>
public interface IDocumentRepository
{
    Task<int> Complete(IUserContext userMatrix, CancellationToken token);

    bool DocumentExists(string fileIdentifier);

    void SaveDocument(Document file);

    Task<Document> FindDocument(Guid publicId, CancellationToken token);

    Task<Document> FindDocument(Guid publicId, bool isDeleted, CancellationToken token);

    Task<Document> FindDocument(Guid publicId, bool isDeleted, bool isPublic, bool isEnabled, CancellationToken token);

    Task<Document> FindDocument(Guid publicId, bool isDeleted, Guid createdBy, CancellationToken token);

    Task RemoveDocument(Guid publicId, CancellationToken token);

    Task Cleanup(CancellationToken token);

    IQueryable<Document> DocumentQuery { get; }
}