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
using RentApp.Persistance.UnitOfWork.Interface;
using RentApp.Models;

namespace RentApp.Controllers
{
    public class VehiclesController : ApiController
    {
   


        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<Vehicle> GetVehicles()
        {
            return unitOfWork.Vehicles.GetAll();
        }

        [ResponseType(typeof(Vehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetVehicles(int id)
        {
            lock (unitOfWork.Vehicles)
            {
                Vehicle rent = unitOfWork.Vehicles.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                return Ok(rent);
            }
        }

        [HttpGet]
        [Route("api/Vehicles/un/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult MakeUnavailable(int id)
        {
            lock (unitOfWork.Vehicles)
            {
                var vehicles = unitOfWork.Vehicles.GetAll();

                var vehicle = new Vehicle();

                foreach (var ve in vehicles)
                {
                    if (ve.Id == id)
                    {
                        vehicle = ve;
                    }
                }

                vehicle.Unvailable = true;

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

        [HttpGet]
        [Route("api/Vehicles/available/{id}")]
        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult MakeAvailable(int id)
        {
            lock (unitOfWork.Vehicles)
            {
                var vehicles = unitOfWork.Vehicles.GetAll();

                var vehicle = new Vehicle();

                foreach (var ve in vehicles)
                {
                    if (ve.Id == id)
                    {
                        vehicle = ve;
                    }
                }

                vehicle.Unvailable = false;

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

        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutVehicle(int id, Vehicle rent)
        {
            lock (unitOfWork.Vehicles)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != rent.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.Vehicles.Update(rent);
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

        [ResponseType(typeof(Vehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostVehicles(VehicleBindingModel vehicleBM)
        {
            lock (unitOfWork.Vehicles)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var type = new TypeOfVehicle();

                var types = unitOfWork.Types.GetAll();

                foreach (var t in types)
                {
                    if (t.Name == vehicleBM.Type)
                    {
                        type = t;
                    }
                }

                var ser = new Service();

                var services = unitOfWork.Services.GetAll();

                foreach (var s in services)
                {
                    if (s.Name == vehicleBM.ServiceName)
                    {
                        ser = s;
                    }
                }

                var vehicle = new Vehicle()
                {
                    Deleted = vehicleBM.Deleted,
                    Description = vehicleBM.Description,
                    Images = vehicleBM.Images,
                    Manufactor = vehicleBM.Manufactor,
                    Model = vehicleBM.Model,
                    PricePerHour = (decimal)vehicleBM.PricePerHour,
                    Type = type,
                    Unvailable = vehicleBM.Unvailable,
                    Year = (int)vehicleBM.Year
                };


                ser.Vehicles.Add(vehicle);

                unitOfWork.Vehicles.Add(vehicle);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = vehicle.Id }, vehicle);
            }
        }

        [ResponseType(typeof(Vehicle))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteVehicle(int id)
        {
            lock (unitOfWork.Vehicles)
            {
                Vehicle rent = unitOfWork.Vehicles.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                rent.Deleted = true;

                unitOfWork.Vehicles.Update(rent);
                unitOfWork.Complete();

                return Ok(rent);
            }
        }

        private bool VehicleExists(int id)
        {
            lock (unitOfWork.Vehicles)
            {
                return unitOfWork.Vehicles.Get(id) != null;
            }
        }
    }
}