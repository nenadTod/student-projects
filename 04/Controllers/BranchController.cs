using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class BranchController : ApiController
    {
        private IUnitOfWork db;

        public BranchController(IUnitOfWork context)
        {
            db = context;
        }

        // GET: api/Services
        [AllowAnonymous]
        public IEnumerable<Branch> GetBranches()
        {
            return db.Branches.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Branch))]
        [AllowAnonymous]
        public IHttpActionResult GetBranch(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }

        
        [Route("api/Branches/BranchesOfService/{serviceId}")]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Branch> GetBranchesOfService(int serviceId)
        {
            return db.Branches.GetAll().Where(b => b.BranchServiceId == serviceId);
        }
        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PutBranch(int id, Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branch.Id)
            {
                return BadRequest();
            }


            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.GetAll().First(u => u.UserName == username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            Service service = db.Services.Get(branch.BranchServiceId);
            if (service.ServiceManagerId != appUser.Id)
            {
                return BadRequest("You are not authorized.");
            }

            if (!service.IsConfirmed)
            {
                return BadRequest("Service is not confirmed yet.");
            }

            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            db.Branches.Update(branch);

            try
            {
                db.Complete();
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

        // POST: api/Services
        [ResponseType(typeof(Branch))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostBranch(Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.GetAll().First(u => u.UserName == username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);
                        
            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            Service service = db.Services.Get(branch.BranchServiceId);
            if(service.ServiceManagerId!=appUser.Id)
            {
                return BadRequest("You are not authorized, not manager of this service.");
            }

            if(!service.IsConfirmed)
            {
                return BadRequest("Service is not confirmed yet.");
            }

            db.Branches.Add(branch);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = branch.Id }, branch);
        }

        [Route("api/Branch/UploadImage")]
        [HttpPost]
        public async Task<HttpResponseMessage> VerifyAppUser()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string branchId = provider.FormData.GetValues("Id")[0];
                int id = Int32.Parse(branchId);
                Branch branch = db.Branches.Get(id);
                MultipartFileData file = provider.FileData[0];
                string destinationFilePath = HttpContext.Current.Server.MapPath("~/Content/Images/BranchImages/");
                destinationFilePath += branchId + ".jpg";
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                File.Copy(file.LocalFileName, destinationFilePath);
                File.Delete(file.LocalFileName);
                branch.Image = @"Content/Images/BranchImages/" + branchId + ".jpg";
                db.Branches.Update(branch);
                db.Complete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Branch))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult DeleteBranch(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return NotFound();
            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.GetAll().First(u => u.UserName == username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            Service service = db.Services.Get(branch.BranchServiceId);
            if (service.ServiceManagerId != appUser.Id)
            {
                return BadRequest("You are not authorized.");
            }

            if(appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            string destinationFilePath = HttpContext.Current.Server.MapPath("~/");
            destinationFilePath += branch.Image;
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            db.Branches.Remove(branch);
            db.Complete();

            return Ok(branch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchExists(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
