using RentApp.Models.Entities;
using RepoDemo.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/ActiveUser")]
    public class ActiveUserController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public ActiveUserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    
        public IHttpActionResult GetActiveUser()
        {
            AppUser a = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);

            return Ok(a);
        }

        public bool IsActivated()
        {
            AppUser a = unitOfWork.AppUsers.GetActiveUser(User.Identity.Name);
            if (a.Activated)
                return true;
            else
                return false;
        }

    }
}