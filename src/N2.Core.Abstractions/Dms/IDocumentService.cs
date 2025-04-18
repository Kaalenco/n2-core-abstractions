namespace N2.Core.Dms;

/// <summary>
/// Abstraction for a document service.
/// </summary>
public interface IDocumentService
{
    Task<IEnumerable<DocumentInformation>> FindDocumentsAsync(
        string search,
        IEnumerable<string> forRoles,
        string processName,
        bool showInactiveDocuments);
    Task<(bool success, string message)> DeleteDocumentAsync(Guid documentIdentifier);
    Task<(bool success, DocumentInformation document)> SaveDocumentAsync(Stream streamedFileContent, DocumentInformation formData);
    Task<(bool success, DocumentInformation document)> UpdateDocumentAsync(Guid documentIdentifier, DocumentInformation formData);
    Task<(bool success, DocumentInformation document)> GetDocumentInformationAsync(Guid documentIdentifier);
}
