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
using System.Web;
using RentApp.Models;
using System.Threading.Tasks;

namespace RentApp.Controllers
{
    [RoutePrefix("api/AdditionalUserOps")]
    public class AppUsersController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private object locker = new object();

        public AppUsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [Route("GetUnbannedManagers")]
        public IEnumerable<AppUser> GetUnbannedManagers()
        {
            return unitOfWork.AppUserRepository.GetUnbannedManagers();
        }

        [Route("GetAllUsers")]
        public IEnumerable<AppUser> GetAllUsers()
        {
            return unitOfWork.AppUserRepository.GetAll();
        }

        [Route("GetUser")]
        public AppUser GetUser(string email)
        {
            AppUser user = unitOfWork.AppUserRepository.Find(u => u.Email == email).FirstOrDefault();
            return user;
        }

        [Authorize(Roles = "Admin")]
        [Route("GetBannedManagers")]
        public IEnumerable<AppUser> GetBannedManagers()
        {
            return unitOfWork.AppUserRepository.GetBannedManagers();
        }

        [Authorize(Roles =("Admin"))]
        [Route("GetAwaitingClients")]
        public IEnumerable<AppUser> GetAwaitingClients()
        {
            return unitOfWork.AppUserRepository.GetAwaitingClients();
        }

        [Authorize(Roles = "Admin")]
        [Route("AuthorizeUser")]
        public string AuthorizeUser([FromBody]string Id)
        {
            lock (locker)
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState).ToString();
                }
                //Get user data, and update activated to true
                AppUser current = unitOfWork.AppUserRepository.Get(Int32.Parse(Id));
                current.Activated = true;

                try
                {
                    unitOfWork.AppUserRepository.Update(current);
                    unitOfWork.Complete();

                    string subject = "Account approval";
                    string desc = $"Dear {current.FullName}, Your account has been approved. Block 8 team.";
                    unitOfWork.AppUserRepository.NotifyViaEmail(current.Email, subject, desc);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest().ToString();
                }

                return "Ok";
            }
        }
    }
}