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
    public class RentsController : ApiController
    {



        private readonly IUnitOfWork unitOfWork;

        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IEnumerable<Rent> GetRents()
        {
            return unitOfWork.Rents.GetAllRents();
        }

        [ResponseType(typeof(Rent))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult GetRents(int id)
        {
            lock (unitOfWork.Rents)
            {
                Rent rent = unitOfWork.Rents.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                return Ok(rent);
            }
        }

        [ResponseType(typeof(void))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PutRent(int id, Rent rent)
        {
            lock (unitOfWork.Rents)
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
                    unitOfWork.Rents.Update(rent);
                    unitOfWork.Complete();
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
        }

        [ResponseType(typeof(Rent))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult PostRents(RentBindingModel rentBM)
        {
            lock (unitOfWork.Rents)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                AppUser appu = new AppUser();
                var appusers = unitOfWork.AppUsers.GetAll();

                foreach (var i in appusers)
                {
                    if (i.Username == User.Identity.Name)
                    {
                        appu = i;
                        break;
                    }
                }

                Vehicle veh = new Vehicle();
                var vehicles = unitOfWork.Vehicles.GetAll();

                foreach (var v in vehicles)
                {
                    if (v.Id == rentBM.VehicleId)
                    {
                        veh = v;
                        break;
                    }
                }


                Rent rent = new Rent()
                {
                    Start = DateTime.Parse(rentBM.Start),
                    End = DateTime.Parse(rentBM.End),
                    Deleted = false,
                    VehicleId = rentBM.VehicleId,
                    GetBranchId = rentBM.GetBranchId,
                    RetBranchId = rentBM.RetBranchId
                };

                appu.Renting.Add(rent);
                veh.Unvailable = true;

                unitOfWork.Rents.Add(rent);
                unitOfWork.AppUsers.Update(appu);
                unitOfWork.Vehicles.Update(veh);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = rent.Id }, rent);
            }
        }

        [ResponseType(typeof(Rent))]
        //[Authorize(Roles="Admin,Manager,AppUser,Client,NotAuthenticated")]
        //[AllowAnonymous]
        public IHttpActionResult DeleteRent(int id)
        {
            lock (unitOfWork.Rents)
            {
                Rent rent = unitOfWork.Rents.Get(id);
                if (rent == null)
                {
                    return NotFound();
                }

                rent.Deleted = true;
                unitOfWork.Rents.Update(rent);
                unitOfWork.Complete();

                return Ok(rent);
            }
        }

        private bool RentExists(int id)
        {
            lock (unitOfWork.Rents)
            {
                return unitOfWork.Rents.Get(id) != null;
            }
        }
    }
}