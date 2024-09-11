using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class VehicleTypesController : ApiController
    {
        private IUnitOfWork db;

        public VehicleTypesController(IUnitOfWork context)
        {
            db = context;
        }
        
        [AllowAnonymous]
        public IEnumerable<VehicleType> GetVehicleTypes()
        {
            return db.VehicleTypes.GetAll();
        }

        [ResponseType(typeof(VehicleType))]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult PostVehicleType(VehicleType vehicleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VehicleType type = db.VehicleTypes.GetAll().FirstOrDefault( t => t.Name == vehicleType.Name);
            if (type != null)
            {
                return BadRequest("Vehicle type already exist.");
            }

            db.VehicleTypes.Add(vehicleType);

            db.Complete();
            return CreatedAtRoute("DefaultApi", new { id = vehicleType.Id }, vehicleType);
        }

    }
}
