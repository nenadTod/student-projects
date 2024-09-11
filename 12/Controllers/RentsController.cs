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
    public class RentsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Rent> GetRents()
        {
            return unitOfWork.Rent.GetAll();
        }

        [ResponseType(typeof(Rent))]
        public IHttpActionResult GetRent(int id)
        {
            Rent rent = unitOfWork.Rent.Get(id);
            if (rent == null)
            {
                return NotFound();
            }

            return Ok(rent);
        }
        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
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
                unitOfWork.Rent.Update(rent);
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
        
        [ResponseType(typeof(Rent))]
        public IHttpActionResult PostRent(RentBindingModel rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = unitOfWork.AppUser.GetAll();

            var vehicle = unitOfWork.Vehicle.Get(rent.Vehicle);

            bool found = false;

            foreach (var item in users)
            {
                foreach (var item2 in item.Rents)
                {
                    if(item2.Vehicle == vehicle)
                    {
                        if(rent.Start >= item2.Start && rent.Start <= item2.End)
                        {
                            found = true;
                        }
                        else if(rent.End >= item2.Start && rent.End <= item2.End)
                        {
                            found = true;
                        }
                        else if(rent.Start <= item2.Start && rent.End >= item2.End)
                        {
                            found = true;
                        }
                    }
                }
            }

            if (!found)
            {
                if (rent.End <= rent.Start)
                    return BadRequest("End time is lower then start time");

                if (vehicle.Unavailable == true)
                    return BadRequest("Vehicle is in use");
                else
                    vehicle.Unavailable = true;

                var branch = unitOfWork.Branch.Get(rent.Branch);

                Rent rr = new Rent() { Branch = branch, Vehicle = vehicle, Start = rent.Start, End = rent.End};

                var user = unitOfWork.AppUser.Get(rent.User);

                user.Rents.Add(rr);

                unitOfWork.AppUser.Update(user);
                unitOfWork.Vehicle.Update(vehicle);
                unitOfWork.Rent.Add(rr);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = rr.Id }, rent);
            }
            else
            {
                return BadRequest("This vehicle is already reserver at the that time.");
            }
        }

        // DELETE: api/Rents/5
        [ResponseType(typeof(Rent))]
        public IHttpActionResult DeleteRent(int id)
        {
            var ren = unitOfWork.Rent.Get(id);

            if (ren == null)
            {
                return NotFound();
            }

            var listOfUsers = unitOfWork.AppUser.GetAll();
            
            foreach (var item in listOfUsers)
            {
                if (item.Rents.Contains(ren))
                    item.Rents.Remove(ren);
            }

            unitOfWork.Rent.Remove(ren);
            unitOfWork.Complete();

            return Ok(ren);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentExists(int id)
        {
            return unitOfWork.Rent.Get(id) != null;
        }
    }
}