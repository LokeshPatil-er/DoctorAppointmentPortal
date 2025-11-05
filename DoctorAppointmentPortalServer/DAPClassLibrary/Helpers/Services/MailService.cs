using DAPClassLibrary.Helpers.Services;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class MailService
    {
        public async Task<bool> SendEmailResponseAsync(string toEmail, string subject, string bodyHtml)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("no-reply@careconnect.com", "Care Connect");
                mailMessage.To.Add(new MailAddress(toEmail));
                mailMessage.Subject = subject;
                mailMessage.Body = bodyHtml;
                mailMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                await smtp.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(MailService), nameof(SendEmailResponseAsync));
               

               
                return false;
            }
        }
    }
}
