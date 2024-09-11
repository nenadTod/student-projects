using Newtonsoft.Json;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class BranchOfficeController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public BranchOfficeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<BranchOffice> GetBranchOffice()
        {
            return unitOfWork.BranchOffices.GetAll();
        }

        [Route("api/GetBranchOfficesForService/{serviceId}")]
        public IEnumerable<BranchOffice> GetBranchOfficesForService(int serviceId)
        {
            return unitOfWork.BranchOffices.GetBranchOfficesForService(serviceId);
        }

        // GET: api/Services/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetBranchOffice(int id)
        {
            BranchOffice branchOffice = unitOfWork.BranchOffices.Get(id);
            if (branchOffice == null)
            {
                return NotFound();
            }

            return Ok(branchOffice);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchOffice(int id, BranchOffice branchOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branchOffice.BranchOfficeID)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.BranchOffices.Update(branchOffice);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchOfficeExists(id))
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
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult PostBranchOffice()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/branches");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var branchOffice = JsonConvert.DeserializeObject<BranchOffice>(HttpContext.Current.Request.Form[0]);

            Validate(branchOffice);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (httpPostedFile != null)
            {
                // Validate the uploaded image(optional)
                var extionsion = new FileInfo(httpPostedFile.FileName).Extension;
                var fileName = Guid.NewGuid() + extionsion;
                // Get the complete file path
                var fileSavePath = Path.Combine(root, fileName);

                while (File.Exists(fileSavePath))
                {
                    fileName = Guid.NewGuid() + extionsion;
                    fileSavePath = Path.Combine(root, fileName);
                }
                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
                branchOffice.Image = "http://localhost:51111/Content/images/branches/" + fileName;
            }

            unitOfWork.BranchOffices.Add(branchOffice);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = branchOffice.BranchOfficeID }, branchOffice);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(BranchOffice))]
        public IHttpActionResult DeleteBranchOffice(int id)
        {
            BranchOffice branchOffice = unitOfWork.BranchOffices.Get(id);
            if (branchOffice == null)
            {
                return NotFound();
            }

            unitOfWork.BranchOffices.Remove(branchOffice);
            unitOfWork.Complete();

            return Ok(branchOffice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchOfficeExists(int id)
        {
            return unitOfWork.BranchOffices.Get(id) != null;
        }
    }
}
