﻿using System;
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
    public class VehiclesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Vehicle> GetVehicles()
        {
            return unitOfWork.Vehicle.GetAll();
        }

        [AllowAnonymous]
        [Route("api/Vehicle/ReturnVehiclesByServer")]
        [HttpGet]
        public List<Vehicle> ReturnVehiclesByServer(int model)
        {
            //int id = Int32.Parse(model);
            var service = unitOfWork.Services.Get(model);
            List<Vehicle> lista = new List<Vehicle>();

            foreach (var item in service.Vehicles)
            {
                lista.Add(item);
            }

            return lista;
        }

        [Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [Route("api/Vehicles/Unavailable")]
        [HttpGet]
        public IHttpActionResult Unavailable(int id)
        {
            var vehicle = unitOfWork.Vehicle.Get(id);
            var rents = unitOfWork.Rent.GetAll();

            foreach (var item in rents)
            {
                if (item.Vehicle == vehicle)
                {
                    if (item.End >= DateTime.Now)
                        return BadRequest("Vehicle is in use!"); 
                }
            }

            if (vehicle.Unavailable == false)
                vehicle.Unavailable = true;
            else
                vehicle.Unavailable = false;

            unitOfWork.Vehicle.Update(vehicle);
            unitOfWork.Complete();

            return Ok(vehicle);
        }

        [Authorize(Roles = "Admin, Manager, AppUser")]
        [AllowAnonymous]
        [Route("api/Vehicles/SearchVehicle")]
        [HttpGet]
        public List<Vehicle> SearchVehicle(string name, string opt)
        {
            var vehicles = unitOfWork.Vehicle.GetAll();
            List<Vehicle> lista = new List<Vehicle>();
            int number = 0;

            if (opt == "Price")
                number = Convert.ToInt32(name);

            foreach (var item in vehicles)
            {
                if (item.Unavailable == true)
                    continue;

                if (opt == "Model")
                {
                    if (item.Model == name)
                        lista.Add(item);
                }
                else if (opt == "Price")
                {
                    if (item.PricePerHour >= number)
                        lista.Add(item);
                }
            }

            return lista;
        }


        public IEnumerable<Vehicle> GetVehicles(int pageIndex, int pageSize)
        {
            var retValue = unitOfWork.Vehicle.GetAll(pageIndex, pageSize);

            return retValue;
        }



        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = unitOfWork.Vehicle.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
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
                unitOfWork.Vehicle.Update(vehicle);
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

        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(VehicleBindingModel vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = unitOfWork.Vehicle.GetAll();

            foreach (var item in list)
            {
                if (item.Model == vehicle.Model)
                    return BadRequest("Already is vehicle with this name: " + vehicle.Model);
            }

            var typeOfVehicles = unitOfWork.TypeOfVehicle.GetAll();
            TypeOfVehicle toV = new TypeOfVehicle();

            foreach (var item in typeOfVehicles)
            {
                if (item.Name == vehicle.TypeOfVehicle)
                {
                    toV = item;
                    break;
                }
            }

            Vehicle vehi = new Vehicle() { Description = vehicle.Description, Images = new List<string>(), Manufactor = vehicle.Manufactor, Model = vehicle.Model, PricePerHour = vehicle.PricePerHour, Unavailable = false, Year = vehicle.Year, Type = toV };

            if (vehicle.Image != "")
                vehi.Images.Add(vehicle.Image);

            toV.Vehicles.Add(vehi);

            var services = unitOfWork.Services.GetAll();
            Services ser = new Services();

            foreach (var item in services)
            {
                if (item.Name == vehicle.ServerName)
                {
                    ser = item;
                    break;
                }
            }

            ser.Vehicles.Add(vehi);

            unitOfWork.Vehicle.Add(vehi);
            unitOfWork.TypeOfVehicle.Update(toV);
            unitOfWork.Services.Update(ser);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = vehi.Id }, vehicle);          
        }

        // DELETE: api/Vehicles/5
        [Authorize(Roles = "Admin, Manager")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            var veh = unitOfWork.Vehicle.Get(id);

            if (veh == null)
            {
                return NotFound();
            }

            var listOfUsers = unitOfWork.AppUser.GetAll();
            var listOfRents = unitOfWork.Rent.GetAll();

            List<Rent> listRentsDelete = new List<Rent>();

            foreach (var r in listOfRents)
            {
                if (r.Branch.Id == veh.Id)
                {
                    if (r.Start <= DateTime.Now && r.End >= DateTime.Now)
                        return BadRequest("Service is in use!");

                    listRentsDelete.Add(r);
                }
            }

            int brojRent = listRentsDelete.Count;

            foreach (var item in listRentsDelete)
            {
                foreach (var item2 in listOfUsers)
                {
                    if (item2.Rents.Contains(item))
                        item2.Rents.Remove(item);
                }
            }

            for (int i = 0; i < brojRent; i++)
            {
                unitOfWork.Rent.Remove(listRentsDelete[i]);
            }

            unitOfWork.Vehicle.Remove(veh);
            unitOfWork.Complete();

            return Ok(veh);
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
            return unitOfWork.Vehicle.Get(id) != null;
        }
    }
}