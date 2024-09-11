using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
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


        // GET: api/Vehicles
        public IEnumerable<Vehicle> GetVehicles()
        {
            return unitOfWork.Vehicles.GetAll();
        }


        public IEnumerable<Vehicle> GetVehiclesBranch(int brId, int pageIndex, int pageSize)
        {
            List<Vehicle> vehicles = new List<Vehicle>(unitOfWork.Vehicles.GetAll());
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Type = unitOfWork.TypeOfVehicles.Get(vehicles[i].TypeId);
               
            }
            return vehicles.Where(b => b.BranchId == brId).OrderBy(b => b.Manufactor).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        }


        // GET: api/Vehicles/5
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


        // PUT: api/Vehicles/5
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != vehicle.CreatorId)
            {
                return BadRequest("Nonauthorized change.\nYou can not modify vehicle you did not add.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
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
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
           
            vehicle.Images=vehicle.Images.Remove(vehicle.Images.Length-1);
            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                return BadRequest("Please fill out all fields and enter correct values.");
            }
            vehicle.CreatorId = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id;
            unitOfWork.Vehicles.Add(vehicle);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = vehicle.Id }, vehicle);
        }


        // DELETE: api/Vehicles/5
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = unitOfWork.Vehicles.Get(id);

            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != vehicle.CreatorId)
            {
                return BadRequest("Nonauthorized delete.\nYou can not delete vehicle you did not add.");
            }


            List<Rent> rents = new List<Rent>(unitOfWork.Rents.GetAll());
            if (rents.Where(i => i.VehicleId == id && i.End < DateTime.Now).FirstOrDefault() != null)
            {
                return BadRequest("Vehicle can't be deleted besause of existing reservation.");
            }
           
            if (vehicle == null)
            {
                return NotFound();
            }

            unitOfWork.Vehicles.Remove(vehicle);
            unitOfWork.Complete();

            return Ok(vehicle);
        }

        private bool VehicleExists(int id)
        {
            return unitOfWork.Vehicles.Get(id) != null;
        }
       
    }
}