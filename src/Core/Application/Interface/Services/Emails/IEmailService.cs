namespace Core.Application.Interface.Services.Emails;

public interface IEmailService
{
    Task SendEmailAsync(
    List<string> to,
    string subject,
    string body,
    List<string>? cc = null,
    List<string>? bcc = null,
    List<(byte[] content, string fileName, string mimeType)>? attachments = null);
}
