using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using RentApp.Persistance;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace RentApp.Controllers
{
    public class AppUsersController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public AppUsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AppUser> GetAppUsers()
        {
            return unitOfWork.AppUsers.GetAll();
        }
      

        [Authorize(Roles = "AppUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            AppUser loggedUser = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);
            if(loggedUser.Id != id)
            {
                return BadRequest("Nonauthorized change\nOnly owner of the profile can change profile information.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill out all fields and enter correct values.");
            }

            if (id != appUser.Id)
            {
                return BadRequest("Nonauthorized change. Only owner of the profile can change profile information.");
            }

            try
            {
                AppUser a = unitOfWork.AppUsers.Get(id);
                a.Birthday = appUser.Birthday;
                a.Email = appUser.Email;
                a.FullName = appUser.FullName;
                a.PersonalDocument = appUser.PersonalDocument;

                unitOfWork.AppUsers.Update(a);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(id))
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


        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostAppUser(AppUser appUser)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.AppUsers.Add(appUser);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = appUser.Id }, appUser);
        }


        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteAppUser(int id)
        {
            AppUser appUser = unitOfWork.AppUsers.Get(id);
            if (appUser == null)
            {
                return NotFound();
            }

            unitOfWork.AppUsers.Remove(appUser);
            unitOfWork.Complete();

            return Ok(appUser);
        }


        private bool AppUserExists(int id)
        {
            return unitOfWork.AppUsers.Get(id) != null;
        }

    }
}