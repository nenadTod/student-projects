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
using RentApp.Persistance.UnitOfWork.Interface;
using RentApp.Models;
using System.Net.Mail;
using System.Text;

namespace RentApp.Controllers
{
    public class AppUsersController : ApiController
    {

        private readonly IUnitOfWork unitOfWork;

        public ApplicationUserManager UserManager { get; private set; }

        private readonly string PASSWORD = "!anuar192DP168";

        public AppUsersController(IUnitOfWork unitOfWork, ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        //GET: api/AppUsers
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<AppUser> GetAppUsers()
        {
           
                return unitOfWork.AppUsers.GetAll();
        }

        //GET: api/AppUsers/5
        [ResponseType(typeof(AppUser))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetAppUser(int id)
        {
            lock (unitOfWork.AppUsers) { 

                    AppUser service = unitOfWork.AppUsers.Get(id);
                    if (service == null)
                    {
                        return NotFound();
                    }

                    return Ok(service);



                } 
               
        }


        [HttpGet]
        [Route("api/AppUsers/RetInfo/{username}")]
        [ResponseType(typeof(AppUser))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetAppUserFromEmail(string username)
        {
            lock (unitOfWork.AppUsers)
            {

                var appusers = unitOfWork.AppUsers.GetAll();

                var appUser = new AppUser();

                foreach (var appu in appusers)
                {
                    if (appu.Username == username)
                    {
                        appUser = appu;
                    }
                }


                if (appUser == null)
                {
                    return NotFound();
                }

                return Ok(appUser);
            }
        }

        [HttpGet]
        [Route("api/AppUsers/blockservice/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult BlockManagerService(int id)
        {
            lock (unitOfWork.AppUsers)
            {
                var users = unitOfWork.AppUsers.GetAll();

                var user = new AppUser();

                foreach (var u in users)
                {
                    if (u.Id == id)
                    {
                        user = u;
                    }
                }

                user.ServiceBlock = true;

                try
                {
                    unitOfWork.AppUsers.Update(user);
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

        [HttpPut]
        [Route("api/AppUsers/Finish/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult FinishProfile(int id, FinishBindingModel finishBM)
        {
            lock (unitOfWork.AppUsers)
            {
                var appusers = unitOfWork.AppUsers.GetAll();

                var appUser = new AppUser();

                foreach (var appu in appusers)
                {
                    if (appu.Id == id)
                    {
                        appUser = appu;
                    }
                }

                appUser.PersonalDocument = finishBM.Image;

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

        //PUT: api/AppUsers/5
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            lock (unitOfWork.AppUsers)
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



        [HttpPut]
        [Route("api/AppUsers/Approve/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult ApproveAppUser(int id, RateBindingModel appUser)
        {

            lock (unitOfWork.AppUsers)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appusers = unitOfWork.AppUsers.GetAll();

                var appuEdit = new AppUser();

                foreach (var appu in appusers)
                {
                    if (appu.Id == id)
                    {
                        appuEdit = appu;
                    }
                }

                appuEdit.Activated = true;

                try
                {
                    unitOfWork.AppUsers.Update(appuEdit);
                    unitOfWork.Complete();

                    //string your_id = "kristijansalaji20@gmail.com";
                    //string your_password = PASSWORD;

                    //SmtpClient client = new SmtpClient();
                    //client.Port = 587;
                    //client.Host = "smtp.gmail.com";
                    //client.EnableSsl = true;
                    //client.Timeout = 10000;
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.UseDefaultCredentials = false;
                    //client.Credentials = new System.Net.NetworkCredential(your_id, your_password);

                    //MailMessage mm = new MailMessage(your_id, "kristijan.salaji@outlook.com");
                    //mm.BodyEncoding = UTF8Encoding.UTF8;
                    //mm.Subject = "CODE FOR FORUM";
                    //mm.Body = "NALOG JE ODOBREN!";
                    //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    //client.Send(mm);

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


        //POST: api/AppUsers
        [ResponseType(typeof(AppUser))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostAppUser(AppUser appUser)
        {
            lock (unitOfWork.AppUsers)
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


        //DELETE: api/AppUsers/5
        [ResponseType(typeof(AppUser))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteAppUser(int id)
        {
            lock (unitOfWork.AppUsers)
            {
                AppUser appUser = unitOfWork.AppUsers.Get(id);
                if (appUser == null)
                {
                    return NotFound();
                }

                appUser.Deleted = true;
                unitOfWork.AppUsers.Update(appUser);
                unitOfWork.Complete();

                return Ok(appUser);
            }
        }

        private bool AppUserExists(int id)
        {
            lock (unitOfWork.AppUsers)
            {
                return unitOfWork.AppUsers.Get(id) != null;
            }
        }
    }
}