using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;

namespace RentApp.Controllers
{
    public class BranchOfficeController : ApiController
    {
        private IUnitOfWork _uow;

        public BranchOfficeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("office/add")]
        [HttpPost]
        public IHttpActionResult Add(BranchOffice office)
        {
            _uow.BranchOffice.Add(office);
            _uow.Complete();
            return Ok(office);
        }

        [Route("office/remove")]
        [HttpDelete]
        public IHttpActionResult Remove(BranchOffice office)
        {
            _uow.BranchOffice.Remove(office);
            _uow.Complete();
            return Ok();
        }

        [Route("office/removeId")]
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            _uow.BranchOffice.Remove(_uow.BranchOffice.Get(id));
            _uow.Complete();
            return Ok();
        }

        [Route("office/all")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var retVal = _uow.BranchOffice.GetAll();
            return Ok(retVal);
        }

        [Route("offce/getrange")]
        [HttpGet]
        public IHttpActionResult GetRange(int pageSize, int pageIndex)
        {
            var retVal = _uow.BranchOffice.GetRange(pageIndex, pageSize);
            return Ok(retVal);
        }

        [Route("office/update")]
        [HttpPut]
        public IHttpActionResult Update(BranchOffice office)
        {
            _uow.BranchOffice.Update(office);
            _uow.Complete();
            return Ok(office);
        }

        [Route("office/uploadToOffice/{id}")]
        [HttpPut]
        public IHttpActionResult UploadImage(int id)
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;

            var postedFile = httpRequest.Files["Image"];

            string extension = Path.GetExtension(postedFile.FileName);
            //Create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + extension;
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);

            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(imageArray);
            string prefix = "data:image/" + extension + ";base64,";
            string base64String = prefix + base64;
            _uow.BranchOffice.UploadImage(base64String, id);
            return Ok(base64String);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}