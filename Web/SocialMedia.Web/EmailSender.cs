using System.Net.Mail;
using System.Net;

namespace SocialMedia
{
    // The interface definition (no need for the 'class' keyword here)
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    // Class implementing the IEmailSender interface
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587; 
        private readonly string _smtpUser = "coooldevs@gmail.com"; 
        private readonly string _smtpPassword = "zchftepuonxvhmvd"; 

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
