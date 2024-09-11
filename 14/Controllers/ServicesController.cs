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

namespace RentApp.Controllers
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public ServicesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        [Route("Aproved")]
        public IEnumerable<Service> GetAprovedServices()
        {
            RADBContext context = new RADBContext();
            return context.Services.Where(x => x.Aproved).ToArray();
        }

        [Route("Unaproved")]
        public IEnumerable<Service> GetUnaprovedServices()
        {
            RADBContext context = new RADBContext();
            return context.Services.Where(x=>!x.Aproved).ToArray();
        }

        [Route("NewService")]
        public int PostNewService(Service service)
        {
            if(service==null)
            {
                return 1;
            }
            RADBContext context = new RADBContext();
            context.Services.Add(service);
            context.SaveChanges();
            return 0;
        }
        
        [Route("AproveService")]
        public int PostAproveNewService(Service ser)
        {
            RADBContext context = new RADBContext();
            Service s = context.Services.FirstOrDefault(x=>x.Id.CompareTo(ser.Id)==0);
            if(s==default(Service))
            {
                return 0;
            }
            s.Aproved = true;
            context.SaveChanges();
            return 1;
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(Guid id)
        {
            Service service = unitOfWork.Services.Find(s => s.Id == id).FirstOrDefault();
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(Guid id, Service service)
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
        public IHttpActionResult PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Services.Add(service);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(Guid id)
        {
            Service service = unitOfWork.Services.Find(s=>s.Id == id).FirstOrDefault();
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

        private bool ServiceExists(Guid id)
        {
            return unitOfWork.Services.Find(s => s.Id == id).FirstOrDefault() != null;
        }
    }
}