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
    [RoutePrefix("api/Types")]
    public class VehicleTypesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private object locker = new object();

        public VehicleTypesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("GetTypes")]
        public IEnumerable<VehicleType> GetVehicleTypes()
        {
            return unitOfWork.VehicleTypes.GetAll();
        }

        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult GetVehicleType(int id)
        {
            VehicleType vehicleType = unitOfWork.VehicleTypes.Get(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return Ok(vehicleType);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicleType(int id, VehicleType vehicleType)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != vehicleType.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.VehicleTypes.Update(vehicleType);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(id))
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

        [Authorize(Roles = "Admin")]
        [Route("PostVehicleType")]
        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult PostVehicleType(VehicleType vehicleType)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                unitOfWork.VehicleTypes.Add(vehicleType);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = vehicleType.Id }, vehicleType);
            }
            
        }

        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult DeleteVehicleType(int id)
        {
            lock (locker)
            {
                VehicleType vehicleType = unitOfWork.VehicleTypes.Get(id);
                if (vehicleType == null)
                {
                    return NotFound();
                }

                unitOfWork.VehicleTypes.Remove(vehicleType);
                unitOfWork.Complete();

                return Ok(vehicleType);
            }
            
        }

        private bool VehicleTypeExists(int id)
        {
            return unitOfWork.VehicleTypes.Get(id) != null;
        }
    }
}