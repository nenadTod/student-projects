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
using System.IO;

namespace RentApp.Controllers
{
    public class BranchesController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public BranchesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Branches
        public IEnumerable<Branch> GetBranches()
        {
            return unitOfWork.Branches.GetAll();
        }


        public IEnumerable<Branch> GetBranchesService(int serviceId, int pageIndex, int pageSize)
        {
            List<Branch> branches=new List<Branch>( unitOfWork.Branches.GetAll().Where(b => b.ServiceId == serviceId).OrderBy(b => b.Address).Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return branches;
        }


        public IEnumerable<Branch> GetBranchesService(int serviceId)
        {
            List<Branch> branches = new List<Branch>(unitOfWork.Branches.GetAll().Where(b => b.ServiceId == serviceId));
            return branches;
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
            if(unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id!=branch.CreatorId)
            {
                return BadRequest("Nonauthorized change.\nYou can not modify branch you did not add.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            if (id != branch.Id)
            {
                return BadRequest("Nonauthorized change.\nYou can not modify branch you did not add.");
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


        // POST: api/Branches
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult PostBranch(Branch branch)
        {
            branch.CreatorId = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id;
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            unitOfWork.Branches.Add(branch);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = branch.Id }, branch);
        }


        // DELETE: api/Branches/5
        [Authorize(Roles = "Manager, Admin")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult DeleteBranch(int id)
        {
          

            Branch branch = unitOfWork.Branches.Get(id);

            if (unitOfWork.AppUsers.GetActiveUser(User.Identity.Name).Id != branch.CreatorId)
            {
                return BadRequest("Nonauthorized delete.\nYou can not delete branch you did not add.");
            }

            List<Vehicle> vehicle = new List<Vehicle>(unitOfWork.Vehicles.GetAll());
            if (vehicle.Where(i => i.BranchId == id).FirstOrDefault() != null)
            {
                return BadRequest("Branch can't be deleted because there are vehicles which belong to this branch.");
            }

            if (branch == null)
            {
                return NotFound();
            }

            unitOfWork.Branches.Remove(branch);
            unitOfWork.Complete();

            return Ok(branch);
        }


        private bool BranchExists(int id)
        {
            return unitOfWork.Branches.Get(id) != null;
        }

    }
}