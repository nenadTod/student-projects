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
    public class BranchesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private static object o = new object();
        public BranchesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // GET: api/Branches
        public IEnumerable<Branch> GetBranches()
        {
            return unitOfWork.Branches.GetAll();
        }

        // GET: api/Branches/5
        [ResponseType(typeof(Branch))]
        public IHttpActionResult GetBranch(int id)
        {
            Branch branch = unitOfWork.Branches.Get(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }

        // PUT: api/Branches/5
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranch(int id, Branch branch)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != branch.Id)
                {
                    return BadRequest();
                }

                try
                {
                    unitOfWork.Branches.Update(branch);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(id))
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

        // POST: api/Branches
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult PostBranch(BranchBindingModel branchBindingModel)
        {
            lock (o)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var branch = new Branch()
                {
                    Address = branchBindingModel.Address,
                    Latitude = branchBindingModel.Latitude,
                    Longitude = branchBindingModel.Longitude,
                    Logo = branchBindingModel.Logo
                };

                var services = unitOfWork.Services.GetAll();

                Service service = new Service();

                foreach (var item in services)
                {
                    if (item.Name == branchBindingModel.ServiceName)
                    {
                        service = item;
                    }
                }

                service.Branches.Add(branch);

                unitOfWork.Branches.Add(branch);
                unitOfWork.Services.Update(service);
                unitOfWork.Complete();

                return CreatedAtRoute("DefaultApi", new { id = branch.Id }, branch);
            }
        }

        // DELETE: api/Branches/5
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult DeleteBranch(int id)
        {
            lock (o)
            {
                Branch branch = unitOfWork.Branches.Get(id);
                if (branch == null)
                {
                    return NotFound();
                }

                List<Rent> rents = unitOfWork.Rents.GetAll().Where(x => x.Branch.Id == id || x.BranchStart.Id == id).ToList();

                unitOfWork.Rents.RemoveRange(rents);
                unitOfWork.Complete();

                unitOfWork.Branches.Remove(branch);
                unitOfWork.Complete();

                return Ok(branch);
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

        private bool BranchExists(int id)
        {
            return unitOfWork.Branches.Get(id) != null;
        }
    }
}