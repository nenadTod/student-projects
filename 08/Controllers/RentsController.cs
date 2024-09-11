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
    public class RentsController : ApiController
    {
        private RADBContext db = new RADBContext();
        private readonly IUnitOfWork unitOfWork;


        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // GET: api/Rents
        public IEnumerable<Rent> GetServices()
        {
            return unitOfWork.Rents.GetAll();
        }

        // GET: api/Rents/5
        [ResponseType(typeof(Rent))]
        public IHttpActionResult GetRent(int id)
        {
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        [HttpGet]
        [Route("api/RentVehicle/CheckRentForVehicle")]
        public IHttpActionResult CheckRent(int ServiceId)
        {
            IEnumerable<Rent> rents = unitOfWork.Rents.GetAll();
            Service s = unitOfWork.Services.Get(ServiceId);
            IEnumerable<Vehicle> vehicles = s.Vehicles;

            foreach(Vehicle v in vehicles)
            {
                foreach (Rent r in rents)
                {
                    if (r.Vehicle.id == v.id)
                    {
                        if (DateTime.Compare(DateTime.Now, r.End.Value) <= 0)
                        {
                            v.Unavailable = true;
                            unitOfWork.Vehicles.Update(v);
                            unitOfWork.Complete();
                            break;
                        }
                        else
                        {
                            v.Unavailable = false;
                            unitOfWork.Vehicles.Update(v);
                            unitOfWork.Complete();
                        }
                    }
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/RentVehicle/Rent")]
        public IHttpActionResult CreateRent(DateTime EndDate, int StartBranchId, int EndBranchId, int VehicleId, int UserId)
        {
            Branch startBranch = unitOfWork.Branches.Get(StartBranchId);
            Branch endBranch   = unitOfWork.Branches.Get(EndBranchId);
            Vehicle vehicle = unitOfWork.Vehicles.Get(VehicleId);
            AppUser user = unitOfWork.AppUsers.Get(UserId);

            Rent rent = new Rent() { Start = DateTime.Now, End = EndDate, Branch = startBranch, ReturnBranch = endBranch, Vehicle = vehicle};
            unitOfWork.Rents.Add(rent);

            user.Rents.Add(rent);
            unitOfWork.AppUsers.Update(user);

            unitOfWork.Complete();

            return Ok();
        }

        // PUT: api/Rents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rent.Id)
            {
                return BadRequest();
            }

            db.Entry(rent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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

        // POST: api/Rents
        [ResponseType(typeof(Rent))]
        public IHttpActionResult PostRent(Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rents.Add(rent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rent.Id }, rent);
        }

        // DELETE: api/Rents/5
        [ResponseType(typeof(Rent))]
        public IHttpActionResult DeleteRent(int id)
        {
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return NotFound();
            }

            db.Rents.Remove(rent);
            db.SaveChanges();

            return Ok(rent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentExists(int id)
        {
            return db.Rents.Count(e => e.Id == id) > 0;
        }
    }
}