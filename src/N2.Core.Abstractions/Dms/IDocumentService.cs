namespace N2.Core.Dms;

/// <summary>
/// Abstraction for a document service.
/// </summary>
public interface IDocumentService
{
    Task<IEnumerable<DocumentInformation>> FindDocuments(
        string search,
        IEnumerable<string> forRoles,
        string processName,
        bool showInactiveDocuments,
        CancellationToken cancellationToken);
    Task<(bool success, string message)> DeleteDocument(Guid documentIdentifier, CancellationToken cancellationToken);
    Task<(bool success, DocumentInformation document)> SaveDocument(Stream streamedFileContent, DocumentInformation formData, CancellationToken cancellationToken);
    Task<(bool success, DocumentInformation document)> UpdateDocument(Guid documentIdentifier, DocumentInformation formData, CancellationToken cancellationToken);
    Task<(bool success, DocumentInformation document)> GetDocumentInformation(Guid documentIdentifier, CancellationToken cancellationToken);
}
