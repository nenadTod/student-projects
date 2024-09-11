using RentApp.Helper;
using RentApp.Hubs;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class NotificationController : ApiController
    {
        private IUnitOfWork db;

        public NotificationController(IUnitOfWork context)
        {
            db = context;
        }

        // GET: api/Services
        [Authorize(Roles = "Admin")]
        public IEnumerable<Notification> GetNotifications()
        {
            return db.Notifications.GetAll();
        }

        [Route("api/Notification/UnseenNotifications")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Notification> GetUnseenNotifications()
        {
            return db.Notifications.GetAll().Where(t => t.Seen == false);
        }

        // GET: api/Services/5
        [ResponseType(typeof(Notification))]
        public IHttpActionResult GetNotification(int id)
        {
            Notification notification = db.Notifications.Get(id);
            if (notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotification(int id, Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notification.Id)
            {
                return BadRequest();
            }
            db.Notifications.Update(notification);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Notification))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostNotification(Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notifications.Add(notification);
            db.Complete();

           
            //return Ok("Hello");

            return CreatedAtRoute("DefaultApi", new { id = notification.Id }, notification);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Notification))]
        public IHttpActionResult DeleteNotification(int id)
        {
            Notification notification = db.Notifications.Get(id);
            if (notification == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notification);
            db.Complete();

            return Ok(notification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int id)
        {
            Notification notification = db.Notifications.Get(id);
            if (notification == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
