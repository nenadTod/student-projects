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
using System.IO;

namespace RentApp.Controllers
{
    public class BranchesController : ApiController
    {
        private RADBContext db = new RADBContext();
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

        [HttpGet]
        [Route("api/GetLogo")]
        public HttpResponseMessage ImageGet(string fileName)
        {
            if(fileName==null)
            {
                fileName = "noimage.jpg";
            }

            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + fileName);
            var ext = System.IO.Path.GetExtension(filePath);
            var contents = System.IO.File.ReadAllBytes(filePath);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(contents);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/" + ext);

            return response;
        }

      //  [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("api/AddBranch")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];
            //Create custom filename
            imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);

            //Parse double num with '.'
            var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
            var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ".";

            Branch branch;

            try
            {
                branch = new Branch { Logo = imageName, Address = httpRequest["Address"], Latitude = Double.Parse(httpRequest["Latitude"], numberFormat), Longitude = Double.Parse(httpRequest["Longitude"], numberFormat) };
            }
            catch
            {
                return null;
            }

            unitOfWork.Branches.Add(branch);
            
            foreach(Service s in unitOfWork.Services.GetAll())
            {
                if(s.Name == httpRequest["ServiceName"] && s.Email == httpRequest["ServiceEmail"])
                {
                    s.Branches.Add(branch);
                    unitOfWork.Services.Update(s);
                }
            }

            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("api/GetBranches")]
        public IEnumerable<Branch> GetBranches(int serviceId)
        {
            Service services = unitOfWork.Services.Get(serviceId);
            if(services!=null)
            {
                return services.Branches;
            }

            return null;
        }

        [Route("api/GetAllBranches")]
        public IEnumerable<Branch> GetAllBranches(int serviceId)
        {
            IEnumerable<Branch> branches = unitOfWork.Branches.GetAll();
            return branches;
        }

        // GET: api/Branches/5
        [ResponseType(typeof(Branch))]
        public IHttpActionResult GetBranch(int id)
        {
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }


        // PUT: api/Branches/5
        [ResponseType(typeof(void))]
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

            db.Entry(branch).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

       // [Authorize(Roles = "Manager")]
        // POST: api/Branches
        [ResponseType(typeof(Branch))]
        public IHttpActionResult PostBranch(Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Branches.Add(branch);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = branch.Id }, branch);
        }

        // DELETE: api/Branches/5
        [ResponseType(typeof(Branch))]
        public IHttpActionResult DeleteBranch(int id)
        {
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return NotFound();
            }

            db.Branches.Remove(branch);
            db.SaveChanges();

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
            return db.Branches.Count(e => e.Id == id) > 0;
        }
    }
}