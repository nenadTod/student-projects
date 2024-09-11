using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class AppUserController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public AppUserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<AppUser> GetUsers()
        {
            return unitOfWork.AppUsers.GetAll();
        }

        // GET: api/Services/5
        [Route("api/User")]
        [HttpGet]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetUser(string email)
        {
            AppUser user = unitOfWork.AppUsers.GetUser(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Services/5
        [Route("api/UserPut")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                unitOfWork.AppUsers.Update(user);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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

        // POST: api/Services
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.AppUsers.Add(user);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteUser(int id)
        {
            AppUser user = unitOfWork.AppUsers.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            unitOfWork.AppUsers.Remove(user);
            unitOfWork.Complete();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return unitOfWork.AppUsers.Get(id) != null;
        }
    }
}
