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
    public class VehicleController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public VehicleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Vehicles
        public IEnumerable<Vehicle> GetVehicles(int idService)
        {
            return unitOfWork.Vehicles.GetAll(idService);
        }

        // GET: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle Vehicle = unitOfWork.Vehicles.Get(id);
            if (Vehicle == null)
            {
                return NotFound();
            }



            return Ok(Vehicle);
        }

        // PUT: api/Vehicles/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(Vehicle Vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.Vehicles.Update(Vehicle);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(Vehicle.Id))
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
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle Vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Vehicles.Add(Vehicle);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = Vehicle.Id }, Vehicle);
        }

        // DELETE: api/Vehicles/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            Vehicle Vehicle = unitOfWork.Vehicles.Get(id);
            if (Vehicle == null)
            {
                return NotFound();
            }

            unitOfWork.Vehicles.Remove(Vehicle);
            unitOfWork.Complete();

            return Ok(Vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(int id)
        {
            return unitOfWork.Vehicles.Get(id) != null;
        }
    }
}
