using System.Net.Mail;
using System.Configuration;

namespace MvcApplication20.Helpers
{
    public static class MailSender
    {
        public static void Send(string subject, string body)
        {
            //1
            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress(ConfigurationManager.AppSettings["messageFrom"]);
            mailObj.To.Add(ConfigurationManager.AppSettings["messageTo"]);
            mailObj.Subject = subject;
            mailObj.Body = body;

            SmtpClient SMTPServer = new SmtpClient("localhost");
            SMTPServer.Send(mailObj);
        }
    }
}