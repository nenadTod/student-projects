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
    public class BranchOfficeController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public BranchOfficeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<BranchOffice> GetBranchOffices(int idService)
        {
            return unitOfWork.BranchOffices.GetAll(idService);
        }

        // GET: api/Services/5
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult GetBranchOffice(int idService, double lat, double lgt)
        {
            BranchOffice office = unitOfWork.BranchOffices.GetBranch(idService, lat, lgt);
            if (office == null)
            {
                return NotFound();
            }

            return Ok(office);
        }

        // PUT: api/Services/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchOffice(int id, BranchOffice office)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != office.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.BranchOffices.Update(office);
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

        // POST: api/Services
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult PostBranchOffice(BranchOffice office)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.BranchOffices.Add(office);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = office.Id }, office);
        }

        // DELETE: api/Services/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult DeleteBranchOffice(int id)
        {
            BranchOffice office = unitOfWork.BranchOffices.Get(id);
            if (office == null)
            {
                return NotFound();
            }

            unitOfWork.BranchOffices.Remove(office);
            unitOfWork.Complete();

            return Ok(office);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchOfficeExists(int id)
        {
            return unitOfWork.BranchOffices.Get(id) != null;
        }
    }
}
