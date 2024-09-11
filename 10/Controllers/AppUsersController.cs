using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork;
using System.Net.Mail;

namespace RentApp.Controllers
{
    public class AppUsersController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private static object o = new object();
        public AppUsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/AppUsers
        public IEnumerable<AppUser> GetAppUsers()
        {
            return unitOfWork.AppUsers.GetAll();
        }

        // GET: api/AppUsers/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetAppUser(int id)
        {
            AppUser appUser = unitOfWork.AppUsers.Get(id);
            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        [Route("api/AppUsers/GetCurrentUser")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCurrentAppUser()
        {
            var username = User.Identity.Name;

            RADBContext db = new RADBContext();
            var user = db.Users.Where(u => u.UserName == username).Include(u1 => u1.AppUser).First();
            var appUser = user.AppUser;

            return Ok(appUser);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/AppUsers/AproveUser/{id}")]
        [HttpPost]
        public IHttpActionResult UserAproving(int id)
        {
            lock (o)
            {
                AppUser user = unitOfWork.AppUsers.Get(id);

                if (user.Activated == true)
                {
                    MailMessage mail = new MailMessage("admin@gmail.com", user.Email);
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("admin@gmail.com", "admin");
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    mail.From = new MailAddress("admin@gmail.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Profile approved";
                    mail.Body = "The account that you have made has been approved by our administrators!";
                    try
                    {
                        client.Send(mail);
                    }
                    catch
                    {

                    }
                }

                return Ok();
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("api/AppUsers/DisapproveUser/{id}")]
        [HttpPost]
        public IHttpActionResult DisapproveUser(int id)
        {
            lock (o)
            {
                AppUser user = unitOfWork.AppUsers.Get(id);

                if (user.Activated == false)
                {
                    MailMessage mail = new MailMessage("admin@gmail.com", user.Email);
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("admin@gmail.com", "admin");
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    mail.From = new MailAddress("admin@gmail.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Profile disapproved";
                    mail.Body = "The account that you have made has been disapproved by our administrators!\n You need to upload valid personal document!";
                    try
                    {
                        client.Send(mail);
                    }
                    catch
                    {

                    }
                }

                user.PersonalDocument = "";

                unitOfWork.AppUsers.Update(user);
                unitOfWork.Complete();

                return Ok();
            }
        }


        [Authorize(Roles = "Admin")]
        [Route("api/AppUsers/GetAppUsersForValidation")]
        [HttpGet]
        //[Authorize]
        public IHttpActionResult GetAppUsersForValidation()
        {
            var username = User.Identity.Name;

            List<AppUser> users = unitOfWork.AppUsers.GetAll().ToList();

            List<AppUser> appUsers = new List<AppUser>();
            foreach (var item in users)
            {
                if (item.PersonalDocument != null && item.PersonalDocument!="" && item.Activated == false)
                {
                    appUsers.Add(item);
                }
            }

            return Ok(appUsers);
        }

        // PUT: api/AppUsers/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != appUser.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.AppUsers.Update(appUser);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
        }


        // POST: api/AppUsers
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostAppUser(AppUser appUser)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                unitOfWork.AppUsers.Add(appUser);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = appUser.Id }, appUser);
            }
        }

        // DELETE: api/AppUsers/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteAppUser(int id)
        {
            lock (o)
            {
                AppUser appUser = unitOfWork.AppUsers.Get(id);
                if (appUser == null)
                {
                    return NotFound();
                }

                unitOfWork.AppUsers.Remove(appUser);
                unitOfWork.Complete();

                return Ok(appUser);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppUserExists(int id)
        {
            return unitOfWork.AppUsers.Get(id) != null;
        }
    }
}