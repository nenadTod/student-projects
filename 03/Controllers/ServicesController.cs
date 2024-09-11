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
using Newtonsoft.Json;

namespace RentApp.Controllers
{
    public class ServicesController : ApiController
    {
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
            Service service = unitOfWork.Services.Get(id);
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

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/services");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var service = JsonConvert.DeserializeObject<Service>(HttpContext.Current.Request.Form[0]);

            Validate(service);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (httpPostedFile != null)
            {
                // Validate the uploaded image(optional)
                var extionsion = new FileInfo(httpPostedFile.FileName).Extension;
                var fileName = Guid.NewGuid() + extionsion;
                // Get the complete file path
                var fileSavePath = Path.Combine(root, fileName);

                while (File.Exists(fileSavePath))
                {
                    fileName = Guid.NewGuid() + extionsion;
                    fileSavePath = Path.Combine(root, fileName);
                }
                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
                service.Logo = "http://localhost:51111/Content/images/services/" + fileName;
            }        
          
            unitOfWork.Services.Add(service);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = unitOfWork.Services.Get(id);
            if (service == null)
            {
                return NotFound();
            }

            unitOfWork.Services.Remove(service);
            unitOfWork.Complete();

            return Ok(service);
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