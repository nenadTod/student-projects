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
using System.Web;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Collections;
using System.Net.Mail;

namespace RentApp.Controllers
{
    public class AppUsersController : ApiController
    {
        private RADBContext db = new RADBContext();
        private readonly IUnitOfWork unitOfWork;


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
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        // PUT: api/AppUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appUser.Id)
            {
                return BadRequest();
            }

            db.Entry(appUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        [Route("api/AppUsers/GetCurrentUser")]
        [HttpGet]
        public IHttpActionResult GetCurrentUser()
        {
            try
            {
                var username = User.Identity.Name;
                RADBContext db = new RADBContext();
                var user = db.Users.Where(u => u.UserName == username).Include(u1 => u1.AppUser).First();
                var appUser = user.AppUser;
                return Ok(appUser);
            }
            catch
            {
                return Ok();
            }
        }

        [Route("api/AppUsers/GetAppUsers")]
        [HttpGet]
        public IHttpActionResult GetNewUsers()
        {
            IEnumerable<AppUser> users = unitOfWork.AppUsers.GetAll();
            List<AppUser> newUsers = new List<AppUser>();

            foreach(AppUser u in users)
            {
                if(u.IsProcessed==false && u.PersonalDocument!=null)
                {
                    newUsers.Add(u);
                }
            }

            return Ok(newUsers);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/AppUsers/AcceptUser")]
        [HttpGet]
        public IHttpActionResult AppUserConfirmation(int id, bool isAccepted)
        {
            AppUser user = unitOfWork.AppUsers.Get(id);
            user.IsAccepted = isAccepted;
            user.IsProcessed = true;
            unitOfWork.AppUsers.Update(user);
            unitOfWork.Complete();

            if (user.IsAccepted == true)
            {
                //prvi parametar sendFrom,drugi sendTo
                MailMessage mail = new MailMessage("foksfak@gmail.com", "foksfak@gmail.com");
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("foksfak@gmail.com", "nadvoznjak");
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                mail.From = new MailAddress("foksfak@gmail.com");
                mail.To.Add("foksfak@gmail.com");
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

        [HttpPost]
        [Route("api/AppUsers/EditUser")]
        public HttpResponseMessage EditUser()
        {
            var httpRequest = HttpContext.Current.Request;

            string imageName = null;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];

            AppUser appUser = unitOfWork.AppUsers.Get(Int32.Parse(httpRequest["AppUserId"]));
            appUser.FullName = httpRequest["FullName"];
            appUser.BirthDay = DateTime.Parse(httpRequest["BirthDay"]);
            appUser.Email = httpRequest["Email"];

            if (appUser.PersonalDocument == null || appUser.PersonalDocument =="")
            {
                imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
                postedFile.SaveAs(filePath);
                appUser.PersonalDocument = imageName;
            }

            unitOfWork.AppUsers.Update(appUser);
            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // POST: api/AppUsers
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostAppUser(AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppUsers.Add(appUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appUser.Id }, appUser);
        }

        // DELETE: api/AppUsers/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteAppUser(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return NotFound();
            }

            db.AppUsers.Remove(appUser);
            db.SaveChanges();

            return Ok(appUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppUserExists(int id)
        {
            return db.AppUsers.Count(e => e.Id == id) > 0;
        }
    }
}