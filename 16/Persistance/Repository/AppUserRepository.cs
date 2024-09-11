using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using static Thinktecture.IdentityModel.Constants.JwtConstants;
using System.IdentityModel.Tokens;

namespace RentApp.Persistance.Repository
{
    public class AppUserRepository : Repository<AppUser, int>, IAppUserRepository
    {
        public AppUserRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<AppUser> GetAll(int pageIndex, int pageSize)
        {
            return Context.AppUsers.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        // Vraca sve menadzere koji nisu banovani
        public IEnumerable<AppUser> GetUnbannedManagers()
        {
            return new List<AppUser>() { new AppUser() { FullName = "Onaj koji nije banovan" }, new AppUser() { FullName = "Onaj koji nije banovan 2" } };
        }

        public bool UserExistsByEmail(string email)
        {
            var user = Context.AppUsers.Where(u => u.Email == email).ToList().FirstOrDefault();
            return user != null ? true : false;
        }
        

        // Vraca sve menadzere koji su banovani
        public IEnumerable<AppUser> GetBannedManagers()
        {
            return new List<AppUser>() { new AppUser() { FullName = "Banovan menadzer 1" } };
        }

        // Vraca sve usere koji cekaju odobrenje naloga
        public IEnumerable<AppUser> GetAwaitingClients()
        {
            return Context.AppUsers.Where(a => a.Activated == false && !String.IsNullOrEmpty(a.Image)).ToList();
        }

        public JwtSecurityToken DecodeJwt(string protectedToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadToken(protectedToken) as JwtSecurityToken;

            return decodedToken;
        }

        public bool NotifyViaEmail(string targetEmail, string subject, string body)
        {
            string mailTo = targetEmail;
            string mailFrom = "capsulelink@gmail.com";
            string pass = "AosOqPB3h74y";

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Timeout = 10000;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailFrom, pass);

                MailMessage mm = new MailMessage(mailFrom, mailTo);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.Subject = subject;
                mm.Body = body;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, mail did not send");
                return false;
            }
        }

        protected RADBContext Context { get { return RADBContext.Create(); } }
    }
}