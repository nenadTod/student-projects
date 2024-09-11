using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    public class VehicleController : ApiController
    {
        IUnitOfWork uow;

        public VehicleController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [Route("vehicle/add")]
        [HttpPost]
        public IHttpActionResult Add(Vehicle vehicle)
        {
            uow.Vehicles.Add(vehicle);
            uow.Complete();
            return Ok();
        }

        [HttpPut]
        [Route("vehicle/update")]
        public IHttpActionResult Update(Vehicle vehicle)
        {
            uow.Vehicles.Update(vehicle);
            uow.Complete();
            return Ok(vehicle);
        }

        [HttpDelete]
        [Route("vehicle/remove")]
        public IHttpActionResult Remove(int id)
        {
            Vehicle v = uow.Vehicles.Get(id);
            uow.Vehicles.Remove(v);
            uow.Complete();
            return Ok();
        }

        [Route("vehicle/uploadToVehicle/{id}")]
        [HttpPut]
        public IHttpActionResult UploadImageToVehicle(int id)
        {
            var httpRequest = HttpContext.Current.Request;

            var postedFile = httpRequest.Files["Image"];

            string extension = Path.GetExtension(postedFile.FileName);
            //Create custom filename
            var imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + extension;
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);

            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(imageArray);
            string prefix = "data:image/" + extension + ";base64,";
            string base64String = prefix + base64;
            uow.Vehicles.UploadImage(base64String, id);
            return Ok(base64String);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
