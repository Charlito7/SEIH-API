using Core.Application.Interface.Services.Emails;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace Infrastructure.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly string? _smtpServer;
        private readonly int _smtpPort;
        private readonly string? _smtpUser;
        private readonly string? _smtpPass;
        private readonly string? _fromAddress;

        public EmailService(IConfiguration config)
        {
            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
            _smtpPort = 587;
            _smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
            _fromAddress = Environment.GetEnvironmentVariable("NO_REPLY");
        }
        public async Task SendEmailAsync(
     List<string> to,
     string subject,
     string body,
     List<string>? cc = null,
     List<string>? bcc = null,
     List<(byte[] content, string fileName, string mimeType)>? attachments = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_fromAddress));
            to.ForEach(address => email.To.Add(MailboxAddress.Parse(address)));
            cc?.ForEach(address => email.Cc.Add(MailboxAddress.Parse(address)));
            bcc?.ForEach(address => email.Bcc.Add(MailboxAddress.Parse(address)));

            email.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            if (attachments != null)
            {
                foreach (var (content, fileName, mimeType) in attachments)
                {
                    bodyBuilder.Attachments.Add(fileName, content, ContentType.Parse(mimeType));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await smtp.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpUser, _smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
