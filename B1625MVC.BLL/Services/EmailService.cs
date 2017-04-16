using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace B1625MVC.Model.Identity
{
    public class EmailService : IIdentityMessageService
    {
        SmtpClient _client;
        string _from;
        public EmailService(SmtpClient client, string fromEmail)
        {
            _from = fromEmail;
            _client = client;
        }
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage mailMessage = new MailMessage(_from, message.Destination);
            mailMessage.Body = message.Body;
            mailMessage.Subject = message.Subject;
            mailMessage.IsBodyHtml = true;

            return _client.SendMailAsync(mailMessage);
        }
    }
}
