using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.UnitOfWork;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        static HashSet<string> LogedIn;

        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            if (LogedIn == null)
            {
                LogedIn = new HashSet<string>();
            }
            this.unitOfWork = unitOfWork;
        }




        // GET: api/AppUsers
        public IEnumerable<User> GetAppUsers()
        {

            return unitOfWork.AppUsers.GetAll();
        }

        [Route("UnaprovedUsers")]
        public IEnumerable<User> GetUnaprovedUsers()
        {
            RADBContext context = new RADBContext();
            return context._Users.Where(x => !x.Approved);
        }

        [Route("NewUser")]
        public int PostNewUser(User user)
        {
            RADBContext context = new RADBContext();
            if(context._Users.FirstOrDefault(x => x.Email == user.Email) != default(User))
            {
                return 0;
            }
            context._Users.Add(user);
            context.SaveChanges();
            return 1;
        }

        [Route("AproveNewUser")]
        public int PostAproveNewUser(User user)
        {
            if(user.Id==null)
            {
                return 2;
            }
            RADBContext context = new RADBContext();
            User u = context._Users.FirstOrDefault(x => x.Id == user.Id);
            if (u== default(User))
            {
                return 0;
            }
            u.Approved = true;
            context.SaveChanges();
            return 1;
        }

        public class ResponceLogin
        {
            public int responce = 0;
            public User user = null;
        }

        [Route("Login/{email}/{password}")]
        public ResponceLogin GetLogin(string email,string password)
        {
            RADBContext context = new RADBContext();
            User user = context._Users.FirstOrDefault(x => email == x.Email);
            if (user == default(User))
            {
                return new ResponceLogin { responce=1};
            }
            if(LogedIn.Contains(user.Email))
            {
                return new ResponceLogin { responce = 2 };
            }
            if (user.Password == password)
            {
                LogedIn.Add(email);
                return new ResponceLogin { responce = 3, user = user };
            }
            else
            {
                return new ResponceLogin { responce = 4 };
            }
        }

        [Route("Logout/{email}")]
        public bool GetLogout(string email)
        {
            if(LogedIn.Contains(email))
            {
                LogedIn.Remove(email);
                return true;
            }
            return false;
        }

        


        // GET: api/AppUsers/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetAppUser(int id)
        {
            User appUser =  unitOfWork.AppUsers.Get(id);
            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        // PUT: api/AppUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(Guid id, User appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appUser.Id)
            {
                return BadRequest();
            }

          

            try
            {
                unitOfWork.AppUsers.Update(appUser);
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

        // POST: api/AppUsers
        [ResponseType(typeof(User))]
        public IHttpActionResult PostAppUser(User appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.AppUsers.Add(appUser);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = appUser.Id }, appUser);
        }

        // DELETE: api/AppUsers/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteAppUser(Guid id)
        {
            User appUser = unitOfWork.AppUsers.Find(u=> u.Id == id).FirstOrDefault();
            if (appUser == null)
            {
                return NotFound();
            }

            unitOfWork.AppUsers.Remove(appUser);
            unitOfWork.Complete();

            return Ok(appUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppUserExists(Guid id)
        {
            return unitOfWork.AppUsers.Find(u => u.Id == id).FirstOrDefault() != null;
        }
    }
}