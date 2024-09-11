using System.Net;
using System.Net.Mail;

namespace RentApp.Services
{
    public class SmtpService : ISmtpService
    {
        public SmtpService()
        {
            
        }
        public void SendMail(string subject, string body, string mailTo)
        {
            var mail = new MailMessage();
            var client = new SmtpClient("smtp.gmail.com");
            mail.To.Add(mailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress("senaandjelic@gmail.com");
            client.Port = 587;
            client.Credentials = new NetworkCredential("senaandjelic@gmail.com", "referencijalniintegritet");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}