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
    [RoutePrefix("api/BranchOffices")]
    public class BranchOfficesController : ApiController
    {
        //private RADBContext db = new RADBContext();
        private IUnitOfWork unitOfWork;

        public BranchOfficesController(IUnitOfWork context)
        {
            this.unitOfWork = context;

        }


        // GET: api/BranchOffices
        public IEnumerable<BranchOffice> GetBrancheOffices()
        {
            return unitOfWork.BranchOffices.GetAll();
        }






        [Route("FromService/{id}")]
        public IEnumerable<BranchOffice> GetBrancheOffices(Guid id)
        {
            RADBContext context = new RADBContext();
            return context.Services.Include(x=>x.BranchOffices).First(x => x.Id.CompareTo(id) == 0).BranchOffices;
            
        }

        [Route("AddToService/{id}")]
        public int PostBranchOffice(Guid id, BranchOffice branchOffice)
        {
            RADBContext context = new RADBContext();
            Service ser = context.Services.Include(x => x.BranchOffices).First(x => x.Id.CompareTo(id) == 0);
            if(ser==default(Service))
            {
                return -1;
            }
            ser.BranchOffices.Add(branchOffice);
            context.SaveChanges();
            return 0;
        }

        // GET: api/BranchOffices/5
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult GetBranchOffice(Guid id)
        {
            BranchOffice branchOffice = unitOfWork.BranchOffices.Find( b=> b.Id == id).FirstOrDefault();
            if (branchOffice == null)
            {
                return NotFound();
            }

            return Ok(branchOffice);
        }

        // PUT: api/BranchOffices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchOffice(Guid id, BranchOffice branchOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branchOffice.Id)
            {
                return BadRequest();
            }

           

            try
            {
                unitOfWork.BranchOffices.Update(branchOffice);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchOfficeExists(id))
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

        // POST: api/BranchOffices
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult PostBranchOffice(BranchOffice branchOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.BranchOffices.Add(branchOffice);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = branchOffice.Id }, branchOffice);
        }

        // DELETE: api/BranchOffices/5
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult DeleteBranchOffice(Guid id)
        {
            BranchOffice branchOffice = unitOfWork.BranchOffices.Find(b=> b.Id == id).FirstOrDefault();
            if (branchOffice == null)
            {
                return NotFound();
            }

            unitOfWork.BranchOffices.Remove(branchOffice);
            unitOfWork.Complete();

            return Ok(branchOffice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchOfficeExists(Guid id)
        {
            return unitOfWork.BranchOffices.Find(b => b.Id == id).FirstOrDefault() != null;
        }
    }
}