namespace N2.Core;

public interface IEmailService
{
    Task<int> SendTestEmailAsync();
    Task<int> SendEmailAsync(string senderAddress, string recipientAddress, string subject, string htmlContent, string plainTextContent);
    Task<int> SendEmailAsync(string recipientAddress, string subject, string htmlContent);
}
