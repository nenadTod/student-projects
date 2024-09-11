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
using System.Text;

namespace RentApp.Controllers
{
    public class RentsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private static object o = new object();
        public RentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Rents
        public IEnumerable<Rent> GetRents()
        {
            return unitOfWork.Rents.GetAll();
        }

        [Route("api/RentsByUserId/{email}")]
        [HttpGet]
        public IHttpActionResult GetRentsByUserId(string email)
        {
            List<AppUser> users = unitOfWork.AppUsers.GetAll().Where(x => x.Email == email).ToList();

            var rents = users[0].Rents;

            return Ok(rents);
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
        [Authorize(Roles = "Manager, Admin, AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRent(int id, Rent rent)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != rent.Id)
                {
                    return BadRequest();
                }

                var rents = unitOfWork.Rents.GetAll();

                Rent r = new Rent();

                foreach(var item in rents)
                {
                    if(item.Id == id)
                    {
                        r = item;
                    }
                }

                r.Paid = true;

                try
                {
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

                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // DELETE: api/Rents/5
        [Authorize(Roles = "Manager, Admin, AppUser")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult DeleteRent(int id)
        {
            lock (o)
            {
                var ren = unitOfWork.Rents.Get(id);

                if (ren == null)
                {
                    return NotFound();
                }

                var listOfUsers = unitOfWork.AppUsers.GetAll();

                foreach (var item in listOfUsers)
                {
                    if (item.Rents.Contains(ren))
                    {
                        item.Rents.Remove(ren);
                        unitOfWork.AppUsers.Update(item);
                    }
                }

                var transactions = unitOfWork.Transactions.GetAll();

                var transaction = new Transaction();

                foreach (var item in transactions)
                {
                    if(item.Rent.Id == id)
                    {
                        transaction = item;
                        unitOfWork.Transactions.Remove(transaction);
                    }
                }

                unitOfWork.Rents.Remove(ren);
                unitOfWork.Complete();

                return Ok(ren);
            }
        }

        // POST: api/Rents
        [Authorize(Roles = "Manager, Admin, AppUser")]
        [ResponseType(typeof(Rent))]
        public IHttpActionResult PostRent(RentBindingModel rentBindingModel)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool found = false;

                var branches = unitOfWork.Branches.GetAll();

                Branch branch = new Branch();
                Branch branchStart = new Branch();

                foreach (var item in branches)
                {
                    if (item.Address == rentBindingModel.Branch)
                    {
                        branch = item;
                    }
                }

                foreach (var item in branches)
                {
                    if (item.Address == rentBindingModel.BranchStart)
                        branchStart = item;
                }

                var vehicles = unitOfWork.Vehicles.GetAll();

                Vehicle vehicle = new Vehicle();

                foreach (var item in vehicles)
                {
                    if (item.Id == rentBindingModel.Vehicle.Id)
                    {
                        vehicle = item;
                    }
                }

                var username = User.Identity.Name;

                var users = unitOfWork.AppUsers.GetAll();

                List<DateTime> startDates = new List<DateTime>();
                List<DateTime> endDates = new List<DateTime>();

                foreach(var item in users)
                {
                    foreach(var item2 in item.Rents)
                    {
                        if(item2.Vehicle == vehicle)
                        {
                            startDates.Add(item2.Start);
                            endDates.Add(Convert.ToDateTime(item2.End));
                            if (rentBindingModel.Start >= item2.Start && rentBindingModel.Start <= item2.End)
                            {
                                //onda ne moze
                                found = true;
                            }
                            else if (rentBindingModel.End >= item2.Start && rentBindingModel.End <= item2.End)
                            {
                                //onda ne moze
                                found = true;
                            }
                            else if (rentBindingModel.Start <= item2.Start && rentBindingModel.End >= item2.End)
                            {
                                //onda ne moze
                                found = true;
                            }
                        }
                    }
                }

                if(!found)
                {
                    AppUser appUser = new AppUser();

                    foreach (var item in users)
                    {
                        if (item.Email == rentBindingModel.Email)
                        {
                            appUser = item;
                        }
                    }

                    Rent rent = new Rent();
                    rent.Branch = branch;
                    rent.BranchStart = branchStart;
                    rent.End = rentBindingModel.End;
                    rent.Start = rentBindingModel.Start;
                    rent.Vehicle = vehicle;
                    //rent.Paid = false;

                    appUser.Rents.Add(rent);
                    unitOfWork.AppUsers.Update(appUser);
                    unitOfWork.Rents.Add(rent);
                    unitOfWork.Complete();

                    return CreatedAtRoute("DefaultApi", new { id = rent.Id }, rent);

                }

                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("This vehicle is already reserved by another users in these period of times: ");
                    for (int i = 0; i < startDates.Count; i++)
                    {
                        stringBuilder.AppendLine("Start date: " + startDates[i] + ", end date: " + endDates[i]);
                    }
                    return BadRequest(stringBuilder.ToString());
                }
            }
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
            return unitOfWork.Rents.Get(id) != null;
        }
    }
}