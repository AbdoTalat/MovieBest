using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MovieBest.BLL.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly string _fromEmail;
        private readonly string _fromPassword;

		public EmailSender(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:Email"];
            _fromPassword = configuration["EmailSettings:Password"];
		}

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlMessage
            };
            msg.To.Add(email);

            using var smtpClient = new SmtpClient("smtp.ethereal.email")
            {
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_fromEmail, _fromPassword)
                
            };

            try
            {
                await smtpClient.SendMailAsync(msg);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
