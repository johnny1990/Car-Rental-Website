using CarRentalWebsite.SmtpService;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;


namespace CarRentalWebsite.Services
{
    public class Smtp
    {
        readonly SmtpOptions _smtpOptions;
        public Smtp(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public void SendSingle(string subject, string htmlBody, string textBody,
                                string toName, string toAddress,
                                string fromName, string fromAddress)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_smtpOptions.SmtpAddress);
                client.Authenticate(_smtpOptions.SmtpUsername, _smtpOptions.SmtpPassword);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                bodyBuilder.TextBody = textBody;

                var message = new MimeMessage();
                message.Body = bodyBuilder.ToMessageBody();
                message.From.Add(new MailboxAddress(fromName, fromAddress));
                message.To.Add(new MailboxAddress(toName, toAddress));
                message.Subject = subject;
                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
