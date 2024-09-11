using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Linq;
using System;

namespace RentApp.Controllers
{
    public class RentsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        public static readonly object LockObject = new object();

        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // GET: api/Rents
        public IEnumerable<Rent> GetRents()
        {
           int id= unitOfWork.AppUsers.GetAll().Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            List<Rent> rents=new List<Rent>(unitOfWork.Rents.GetAll().Where(r=>r.UserId==id));
          
            for (int i = 0; i < rents.Count; i++)
            {
                rents[i].Vehicle = unitOfWork.Vehicles.Get(rents[i].VehicleId);
                rents[i].ReturnBranch = unitOfWork.Branches.Get(rents[i].ReturnBranchId);
            }

            return rents;
        }


        // GET: api/Rents/5
        [ResponseType(typeof(Rent))]
        public IHttpActionResult GetRent(int id)
        {
            Rent rent = unitOfWork.Rents.Get(id);
            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }


        // PUT: api/Rents/5
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            if (id != rent.Id)
            {
                return BadRequest();
            }


            if (DateTime.Compare((DateTime)rent.Start, (DateTime)rent.End) > 0)
            {
                return BadRequest("End date must be smaller then start date.");
            }
            else if(DateTime.Compare((DateTime)rent.End, DateTime.Now) < 0)
            {
                return BadRequest("End date is not valid.");
            }

            int count = unitOfWork.Rents.GetAll().Where(r => r.VehicleId == rent.VehicleId && (r.End < rent.Start || rent.End < r.Start)).Count();
            int countV = unitOfWork.Rents.GetAll().Where(r => r.VehicleId == rent.VehicleId).Count();
            if (count > 0 || countV == 1)
            {
                try
                {
                    Rent r = unitOfWork.Rents.Get(id);
                    r.End = rent.End;
                    r.Start = rent.Start;

                    unitOfWork.Rents.Update(r);
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

                return Ok("Rent is succesfully updated.");
            }
            else
            {
                return BadRequest("Vehicle is rented in that period.");
            }

        }


        // POST: api/Rents
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult PostRent(Rent rent)
        {
            lock (LockObject)
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest("Please fill out all fields and enter correct values.");
                }

                if (DateTime.Compare((DateTime)rent.Start, (DateTime)rent.End) > 0)
                {
                    return BadRequest("End date must be smaller then start date.");
                }
                else if(DateTime.Compare((DateTime)rent.End, DateTime.Now) < 0)
                {
                    return BadRequest("End date is not valid.");
                }

                int count = unitOfWork.Rents.GetAll().Where(r => r.VehicleId == rent.VehicleId && (r.End < rent.Start || rent.End < r.Start)).Count();
                int countV = unitOfWork.Rents.GetAll().Where(r => r.VehicleId == rent.VehicleId).Count();
                if (count > 0 || countV == 0)
                {
                    unitOfWork.Rents.Add(rent);

                    Vehicle vehicle = unitOfWork.Vehicles.Get(rent.VehicleId);
                    vehicle.BranchId = rent.ReturnBranchId;

                    unitOfWork.Vehicles.Update(vehicle);
                    unitOfWork.Complete();

                    return Ok("Rent of vehicle is succesfully made.");
                }
                else
                {
                    return BadRequest("Vehicle is rented in that period.");
                }
            }
        }


        // DELETE: api/Rents/5
        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult DeleteRent(int id)
        {
            Rent rent = unitOfWork.Rents.Get(id);
            if (rent == null)
            {
                return NotFound();
            }

            unitOfWork.Rents.Remove(rent);
            unitOfWork.Complete();

            return Ok(rent);
        }


        private bool RentExists(int id)
        {
            return unitOfWork.Rents.Get(id) != null;
        }
    }
}