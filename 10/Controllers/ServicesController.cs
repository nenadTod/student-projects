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
using RentApp.Hubs;

namespace RentApp.Controllers
{
    public class ServicesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private static object o = new object();
        public ServicesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<Service> GetServices()
        {

            var retValue = unitOfWork.Services.GetAll();
            List<Service> services = new List<Service>();
            foreach (var item in retValue)
            {
                if (item.Activated == true)
                {
                    services.Add(item);
                    foreach (var item2 in item.Vehicles)
                    {
                        item2.Images = new List<string>();
                        string[] str = item2.VehicleImagesBase.Split(';');
                        foreach (var img in str)
                        {
                            if (img != "")
                                item2.Images.Add(img);
                        }
                    }
                }
            }

            return services;
            
        }

        [Route("api/Services/GetNumberOfServices")]
        [HttpGet]
        public int GetNumberOfServices()
        {
            var retValue = unitOfWork.Services.GetAll();

            return retValue.Count();
        }


        [Route("api/Services/GetServicesForValidation")]
        [HttpGet]
        public IEnumerable<Service> GetServicesForValidation()
        {

            var retValue = unitOfWork.Services.GetAll();
            List<Service> services = new List<Service>();
            foreach (var item in retValue)
            {
                if(item.Activated == false)
                {
                    services.Add(item);
                }
            }

            return services;

        }

        public IEnumerable<Service> GetServices(int pageIndex, int pageSize)
        {
            var retValue = unitOfWork.Services.GetAll(pageIndex,pageSize);

            List<Service> services = new List<Service>();
            foreach (var item in retValue)
            {
                if (item.Activated == true)
                {
                    services.Add(item);
                    foreach (var item2 in item.Vehicles)
                    {
                        item2.Images = new List<string>();
                        string[] str = item2.VehicleImagesBase.Split(';');
                        foreach (var img in str)
                        {
                            if (img != "")
                                item2.Images.Add(img);
                        }
                    }
                }
            }

            return services;

        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {

            Service service = unitOfWork.Services.Get(id);

            if (service == null)
            {
                return NotFound();
            }

            foreach (var item in service.Vehicles)
            {
                item.Images = new List<string>();
                string[] str = item.VehicleImagesBase.Split(';');
                foreach (var img in str)
                {
                    if (img != "")
                        item.Images.Add(img);
                }
            }

            return Ok(service);
        }

        [Route("api/Services/UpdateServiceRating/{id}/{service}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager, Admin, AppUser")]
        public IHttpActionResult UpdateServiceRating(int id, Service service)
        {
            lock (o)
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != service.Id)
                {
                    return BadRequest();
                }

                var services = unitOfWork.Services.GetAll();
                var ser = new Service();

                foreach (var item in services)
                {
                    if(item.Id == id)
                    {
                        ser = item;
                    }
                }

                ser.Rating = service.Rating;

                try
                {
                    unitOfWork.Services.Update(ser);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(id))
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

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager, Admin, AppUser")]
        public IHttpActionResult PutService(int id, Service service)
        {
            lock(o)
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != service.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.Services.Update(service);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(id))
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

        // POST: api/Services
        [ResponseType(typeof(Service))]
        [Authorize(Roles = "Manager, Admin")]
        public IHttpActionResult PostService(Service service)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                service.Owner = User.Identity.Name;

                unitOfWork.Services.Add(service);
                unitOfWork.Complete();

                NotificationsHub.NotifyAdmin("New service added and requires aproval!");

                return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
            }
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        [Authorize(Roles = "Manager, Admin")]
        public IHttpActionResult DeleteService(int id)
        {
            lock (o)
            {
                Service service = unitOfWork.Services.Get(id);
                if (service == null)
                {
                    return NotFound();
                }

                List<Vehicle> vehicles = unitOfWork.Vehicles.GetAll().Where(x => service.Vehicles.Contains(x)).ToList();
                List<Rent> rents = unitOfWork.Rents.GetAll().ToList();
                List<Rent> rentsToDelete = new List<Rent>();

                foreach(var item in rents)
                {
                    foreach(var item2 in vehicles)
                    {
                        if(item.Vehicle.Id == item2.Id)
                        {
                            rentsToDelete.Add(item);
                        }
                    }
                }

                unitOfWork.Rents.RemoveRange(rentsToDelete);
                unitOfWork.Complete();

                rents.Clear();
                rentsToDelete.Clear();

                rents = unitOfWork.Rents.GetAll().ToList();

                foreach (var item in rents)
                {
                    foreach (var item2 in service.Branches)
                    {
                        if (item.Branch.Id == item2.Id || item.BranchStart.Id == item2.Id)
                        {
                            rentsToDelete.Add(item);
                        }
                    }
                }

                unitOfWork.Rents.RemoveRange(rentsToDelete);
                unitOfWork.Complete();

                List<Branch> branches = unitOfWork.Branches.GetAll().Where(x => service.Branches.Contains(x)).ToList();

                List<Comment> comments = unitOfWork.Comments.GetAll().Where(x => x.Service_Id == id).ToList();
                List<Rating> ratings = unitOfWork.Ratings.GetAll().Where(x => x.Service.Id == id).ToList();

                unitOfWork.Comments.RemoveRange(comments);
                unitOfWork.Complete();

                unitOfWork.Ratings.RemoveRange(ratings);
                unitOfWork.Complete();

                unitOfWork.Vehicles.RemoveRange(vehicles);
                unitOfWork.Complete();

                unitOfWork.Branches.RemoveRange(branches);
                unitOfWork.Complete();

                unitOfWork.Services.Remove(service);
                unitOfWork.Complete();

                return Ok(service);
            }
        }

        [Route("api/Services/aproveService/{id}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ServiceAproving(int id)
        {
            lock (o)
            {
                Service service = unitOfWork.Services.Get(id);

                var serviceOwner = service.Owner;

                if (!service.Owner.Contains("@gmail.com"))
                {
                    service.Owner = serviceOwner + "@gmail.com";
                }

                if (service.Activated == true)
                {
                    MailMessage mail = new MailMessage("admin@gmail.com", service.Owner);
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("admin@gmail.com", "admin");
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    mail.From = new MailAddress("admin@gmail.com");
                    mail.To.Add(service.Owner);
                    mail.Subject = "Service approved";
                    mail.Body = "The service that you have made has been approved by our administrators! \n You are now able to add vehicles and branches!";
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

        [Route("api/Services/disapproveService/{id}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ServiceDisapproving(int id)
        {
            lock (o)
            {
                Service service = unitOfWork.Services.Get(id);

                var serviceOwner = service.Owner;

                if (!service.Owner.Contains("@gmail.com"))
                {
                    service.Owner = serviceOwner + "@gmail.com";
                }

                if (service.Activated == false)
                {
                    MailMessage mail = new MailMessage("admin@gmail.com", service.Owner);
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("admin@gmail.com", "admin");
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    mail.From = new MailAddress("admin@gmail.com");
                    mail.To.Add(service.Owner);
                    mail.Subject = "Service disapproved";
                    mail.Body = "The service that you have made has been disapproved by our administrators! \n Please, create service on a proper way!";
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return unitOfWork.Services.Get(id) != null;
        }
    }
}