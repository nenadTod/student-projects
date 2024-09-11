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
using System.Net.Mail;
using RentApp.Hubs;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private RADBContext db;
        private readonly IUnitOfWork unitOfWork;

        public ServicesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<Service> GetServices()
        {
           return unitOfWork.Services.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.Id)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        [Authorize(Roles = "Manager")]
        // POST: api/Services
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
            Service s = new Service();
            s = service;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Services.Add(s);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
        }

        [AllowAnonymous]
        [Route("GetServices")]
        [HttpGet]
        public IHttpActionResult GetNewServices()
        {
            IEnumerable<Service> services = unitOfWork.Services.GetAll();
            List<Service> newServices = new List<Service>();

            foreach (Service s in services)
            {
                if (s.IsProcessed == false)
                {
                    newServices.Add(s);
                }
            }

            return Ok(newServices);
        }

        [Route("Search")]
        [HttpGet]
        public IHttpActionResult GiveComment(string type, decimal price)
        {
            IEnumerable<Vehicle> vehicles = unitOfWork.Vehicles.GetAll();
            List<Vehicle> retVal = new List<Vehicle>();

            if (price <= 0.0m && type != null) 
            {
                retVal = SearchTypeOnly(vehicles, type);
                return Ok(retVal);
            }
            else if(type==null && price>0)
            {
                retVal = SearchPriceOnly(vehicles, price);
                return Ok(retVal);
            }
            else if(type!=null && price>0.0m)
            {
                retVal = SearchFull(vehicles, type, price);
                return Ok(retVal);
            }

            return Ok(retVal);
        }

        public List<Vehicle> SearchTypeOnly(IEnumerable<Vehicle> vehicles, string type)
        {
            List<Vehicle> searchList = new List<Vehicle>();

            foreach (Vehicle v in vehicles)
            {
                if (v.Type.Id.ToString() == type)
                {
                    searchList.Add(v);
                }
            }

            return searchList;
        }

        public List<Vehicle> SearchPriceOnly(IEnumerable<Vehicle> vehicles, decimal price)
        {
            List<Vehicle> searchList = new List<Vehicle>();

            foreach (Vehicle v in vehicles)
            {
                if (v.PricePerHour == price)
                {
                    searchList.Add(v);
                }
            }

            return searchList;
        }

        public List<Vehicle> SearchFull(IEnumerable<Vehicle> vehicles, string type, decimal price)
        {
            List<Vehicle> searchList = new List<Vehicle>();

            foreach (Vehicle v in vehicles)
            {
                if (v.Type.Id.ToString() == type && v.PricePerHour==price)
                {
                    searchList.Add(v);
                }
            }

            return searchList;
        }

        [Route("GiveComment")]
        [HttpGet]
        public IHttpActionResult GiveComment(int userId, int serviceId, string content, bool isNegative)
        {
            Service service = unitOfWork.Services.Get(serviceId);
            AppUser appUser = unitOfWork.AppUsers.Get(userId);

            Comment comment = new Comment { Content = content, IsNegative = isNegative, PostedDate = DateTime.Now, UserKey=appUser.Id};
            service.Comments.Add(comment);
            appUser.Comments.Add(comment);
            unitOfWork.Comments.Add(comment);
            unitOfWork.Services.Update(service);
            unitOfWork.AppUsers.Update(appUser);
            unitOfWork.Complete();
            
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("AproveService")]
        [HttpGet]
        public IHttpActionResult serviceConfirmation(int id, bool isAccepted)
        {
            Service service = unitOfWork.Services.Get(id);
            AppUser au = new AppUser();
            service.IsAccepted = isAccepted;
            service.IsProcessed = true;
            unitOfWork.Services.Update(service);
            unitOfWork.Complete();
            
            if (service.IsAccepted == true)
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
                mail.From= new MailAddress("foksfak@gmail.com");
                mail.To.Add("foksfak@gmail.com");
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

        [HttpGet]
        [Route("GetImage")]
        public HttpResponseMessage ImageGet(string path)
        {
            if (path == null)
            {
                path = "noimage.jpg";
            }

            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + path);
            var ext = System.IO.Path.GetExtension(filePath);
            var contents = System.IO.File.ReadAllBytes(filePath);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(contents);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/" + ext);

            return response;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            //Notify for websocket
            NotificationsHub.NotifyAdmin("New service is added...");

            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];
            //Create custom filename
            imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);

            Service service = new Service { Logo = imageName, Name = httpRequest["Name"], Email = httpRequest["Email"], Description = httpRequest["Description"] };
            unitOfWork.Services.Add(service);
            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("DeleteService")]
        public HttpResponseMessage DeleteService()
        {
            var httpRequest = HttpContext.Current.Request;

            Service service = unitOfWork.Services.Get(Int32.Parse(httpRequest["ServiceId"]));
            service.IsDeleted = true;
            unitOfWork.Services.Update(service);
            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpGet]
        [Route("GetVehicles")]
        public IEnumerable<Vehicle> GetVehicles(int id)
        {
            Service service = unitOfWork.Services.Get(id);
            if(service==null)
            {
                return null;
            }

            return service.Vehicles;
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
            return db.Services.Count(e => e.Id == id) > 0;
        }
    }
}