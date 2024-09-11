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
    [RoutePrefix("api/Vehicles")]
    public class VehiclesController : ApiController
    {
        //private RADBContext db = new RADBContext();
        private IUnitOfWork unitOfWork;

        public VehiclesController(IUnitOfWork context)
        {
            this.unitOfWork = context;
        }

        // GET: api/Vehicles
        public IEnumerable<Vehicle> GetVehicles()
        {
            return unitOfWork.Vehicles.GetAll();
        }

        [Route("FromBranchOffice/{id}")]
        public IEnumerable<Vehicle> GetVehicles(Guid id)
        {
            RADBContext context = new RADBContext();
            return context.BrancheOffices.Include(x => x.Vehicles).First(x => x.Id.CompareTo(id) == 0).Vehicles;
        }

        [Route("AddToBranchOffice/{id}")]
        public int PostVehicle(Guid id, Vehicle vehicle)
        {
            RADBContext context = new RADBContext();
            BranchOffice bo= context.BrancheOffices.Include(x=>x.Vehicles).FirstOrDefault(x => x.Id.CompareTo(id) == 0);
            if(bo==default(BranchOffice))
            {
                return -1;
            }
            bo.Vehicles.Add(vehicle);
            context.SaveChanges();
            return 0;
        }


        // GET: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(Guid id)
        {
            Vehicle vehicle = unitOfWork.Vehicles.Find(v=> v.Id == id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(Guid id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.Id)
            {
                return BadRequest();
            }

        

            try
            {
                unitOfWork.Vehicles.Update(vehicle);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Vehicles.Add(vehicle);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(Guid id)
        {
            Vehicle vehicle = unitOfWork.Vehicles.Find(v=> v.Id == id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            unitOfWork.Vehicles.Remove(vehicle);
            unitOfWork.Complete();

            return Ok(vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(Guid id)
        {
            return unitOfWork.Vehicles.Find(v => v.Id == id).FirstOrDefault() != null;
        }
    }
}