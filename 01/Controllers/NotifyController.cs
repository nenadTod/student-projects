using RentApp.Hubs;
using RentApp.Models.Entities;
using RentApp.Services;
using RepoDemo.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class NotifyController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISmtpService smtpService;

        public NotifyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.smtpService = new SmtpService();
        }


        public IEnumerable<Notification> GetNotifications()
        {
            return unitOfWork.Notifications.GetAll();
        }


        [HttpPost]
        public IHttpActionResult NotifyAdmin([FromUri] string email)
        {
            Notification notification = new Notification();
            notification.Text = "New user registrated: " + email;
            notification.Date = DateTime.Now;

            if (PostNotification(notification))
            {
                NotificationsHub.Notify(notification);
                return Ok("Success");
            }

            NotificationsHub.Notify(null);
            return Ok("Failure");
        }

        [HttpPost]
        public IHttpActionResult NotifyAdmin([FromUri] string serviceName, [FromUri] int id)
        {
            Notification notification = new Notification();
            notification.Text = "New service added with name: " + serviceName + " and id: " + id;
            notification.Date = DateTime.Now;

            if (PostNotification(notification))
            {
                NotificationsHub.Notify(notification);
                return Ok("Success");
            }

            NotificationsHub.Notify(null);
            return Ok("Failure");
        }

        private bool PostNotification(Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            unitOfWork.Notifications.Add(notification);
            unitOfWork.Complete();

            return true;
        }

        [ResponseType(typeof(Notification))]
        public IHttpActionResult Approve([FromUri] int id,[FromUri] bool approved)
        {
            Notification notification = unitOfWork.Notifications.Get(id);
            if (notification == null)
            {
                return NotFound();
            }

            if(notification.Text.Contains("New user registrated"))
            {
                string email = notification.Text.Substring(notification.Text.IndexOf(": ") + 2);
                AppUser app = unitOfWork.AppUsers.GetAll().Where(u => u.Email == email).FirstOrDefault();

                app.Activated = approved;
                unitOfWork.AppUsers.Update(app);
                unitOfWork.Complete();

                if (approved)
                {
                    string body = "Welcome to the Rent a vehicle.\nYou are succesfuly registrated.\nBelow is the LogIn link:\n" + "http://localhost:51680/logIn";
                    //smtpService.SendMail("Welcome", body, email);
                }
                else
                {

                    string body = "You are unsuccesfuly registrated.\nTry to register again.\nBelow is the registration link:\n" + "http://localhost:51680/register";
                    //smtpService.SendMail("Failed", body, email);
                }
            }
            else if(notification.Text.Contains("New service added"))
            {
                string serId = notification.Text.Substring(notification.Text.LastIndexOf(": ") + 2);

                Service ser = unitOfWork.Services.GetAll().Where(s => s.Id == Int32.Parse(serId)).FirstOrDefault();

                ser.Activated = approved;
                unitOfWork.Services.Update(ser);
                unitOfWork.Complete();

                if (approved)
                {
                    string body = "Service you added with name: " + ser.Name + " is added successfully.";
                    //smtpService.SendMail("Success", body, ser.Email);
                }
                else
                {

                    string body = "Service you added with name: " + ser.Name + " is added unsuccessfully.";
                    //smtpService.SendMail("Failed", body, ser.Email);
                }
            }
           

            unitOfWork.Notifications.Remove(notification);
            unitOfWork.Complete();

            return Ok("");
        }

    }
}