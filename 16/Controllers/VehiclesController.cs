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
using RentApp.Models;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Vehicles")]
    public class VehiclesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private object locker = new object();

        public VehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("GetVehicles")]
        public IEnumerable<Vehicle> GetVehicles()
        {
            List<Service> AuthorizedServices = unitOfWork.Services.GetAll().Where(s => s.Authorized == true).ToList();
            List<Vehicle> enabledVehicles = new List<Vehicle>();
            foreach (var service in AuthorizedServices)
            {
                foreach (var vehicle in service.Vehicles)
                {
                    if (!vehicle.Unavailable)
                    {
                        enabledVehicles.Add(vehicle);
                    }
                }
            }
            return enabledVehicles;
        }

        [Route("GetVehicle")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = unitOfWork.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("AddVehicles")]
        public IHttpActionResult AddVehicle(Vehicle vehicle)
        {
            lock (locker)
            {
                this.unitOfWork.Vehicles.Add(vehicle);
                return Ok();
            }
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("PutVehicle")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, VehicleRequestModel vehicle)
        {
            lock (locker)
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
                    var vs = unitOfWork.Vehicles.GetAll();
                    Vehicle v = vs.ToList<Vehicle>().Where(ve => ve.Id == id).ToList().First();
                    v.Id = id;
                    v.Model = vehicle.Model;
                    v.Description = vehicle.Description;
                    v.Manufactor = vehicle.Manufactor;
                    v.Year = vehicle.Year;
                    v.PricePerHour = vehicle.PricePerHour;
                    var types = unitOfWork.VehicleTypes.GetAll();
                    v.Type = (VehicleType)types.ToList<VehicleType>().Where(t => t.Name == vehicle.Type).ToList().First();
                    //v.Images = vehicle.Images;

                    unitOfWork.Vehicles.Update(v);
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
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("PutVehicleAvailability")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicleAvailability(int id, Vehicle vehicle)
        {
            lock (locker)
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
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("PostVehicle")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(VehicleRequestModel vehicle)
        {
            lock (locker)
            {
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Vehicle v = new Vehicle();
                v.Model = vehicle.Model;
                v.Description = vehicle.Description;
                v.Manufactor = vehicle.Manufactor;
                v.Year = vehicle.Year;
                v.PricePerHour = vehicle.PricePerHour;
                var types = unitOfWork.VehicleTypes.GetAll();
                v.Type = (VehicleType)types.ToList<VehicleType>().Where(t => t.Name == vehicle.Type).ToList().First();
                //v.Images = vehicle.Images;

                Service service = unitOfWork.Services.Get(vehicle.ServiceId);

                try
                {
                    unitOfWork.Vehicles.Add(v);
                    service.Vehicles.Add(v);
                    unitOfWork.Complete();

                    return Ok(v.Id);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("DeleteVehicle")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            lock (locker)
            {
                Vehicle vehicle = unitOfWork.Vehicles.Get(id);
                if (vehicle == null)
                {
                    return NotFound();
                }

                var rents = unitOfWork.Rents.GetAll();
                var rentsToDelete = new List<Rent>();
                foreach (var rent in rents)
                {
                    if (rent.Vehicle.Id == id)
                        rentsToDelete.Add(rent);
                }


                unitOfWork.Rents.RemoveRange(rentsToDelete);
                unitOfWork.Complete();

                unitOfWork.Vehicles.Remove(vehicle);
                unitOfWork.Complete();

                return Ok(vehicle);
            }
        }

        [Route("SearchVehicles")]
        public VehiclePaginationResponse SearchVehicles([FromBody]SearchVehicleRequestModel searchVehicleRequest)
        {
            return unitOfWork.Vehicles.SearchVehicles(searchVehicleRequest);
        }

        private bool VehicleExists(int id)
        {
            return unitOfWork.Vehicles.Get(id) != null;
        }
    }
}