using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class UtilityMethods
    {
        public static void SendEmail(string toEmail,string subject,string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConstantData.fromEmail);
                mailMessage.To.Add(new MailAddress(toEmail));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;
                SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential(ConstantData.usernameEmail, ConstantData.passwordEmail);
                client.Host = ConstantData.emailHost;
                client.Port = ConstantData.emailPort;
                client.EnableSsl = ConstantData.enableSsl;
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
