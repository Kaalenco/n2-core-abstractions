namespace N2.Core;

public interface IEmailService
{
    Task<int> SendTestEmail(CancellationToken cancellationToken);
    Task<int> SendEmail(string senderAddress, string recipientAddress, string subject, string htmlContent, string plainTextContent, CancellationToken cancellationToken);
    Task<int> SendEmail(string recipientAddress, string subject, string htmlContent, CancellationToken cancellationToken);
}

public interface ICryptoService
{
    
}