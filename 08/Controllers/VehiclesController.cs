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
    public class VehiclesController : ApiController
    {
        private RADBContext db = new RADBContext();
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        // GET: api/Vehicles
        public IEnumerable<Vehicle> GetServices()
        {
            return unitOfWork.Vehicles.GetAll();
        }

        // GET: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        [Route("api/GetVehicle")]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("api/DisableVehicle")]
        public IHttpActionResult DisableVehicle(int vehicleId)
        {
            Vehicle vehicle = unitOfWork.Vehicles.Get(vehicleId);

            vehicle.Disable = true;
            unitOfWork.Vehicles.Update(vehicle);
            unitOfWork.Complete();

            return Ok(vehicle);
        }

        [ResponseType(typeof(Vehicle))]
        [Route("api/GetAllVehicles")]
        public IHttpActionResult GetAllVehicles()
        {
            IEnumerable<Vehicle> vehicles = unitOfWork.Vehicles.GetAll();
            return Ok(vehicles);
        }

        // PUT: api/Vehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.id)
            {
                return BadRequest();
            }

            db.Entry(vehicle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vehicles.Add(vehicle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vehicle.id }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return Ok(vehicle);
        }

        public IEnumerable<Vehicle> GetVehicles(int pageIndex,int pageSize, int serviceId)
        {
            //var retValue = unitOfWork.Vehicles.GetAll(pageIndex, pageSize);
            Service service = unitOfWork.Services.Get(serviceId);
            List<Vehicle> vehicles = service.Vehicles;

            vehicles = new List<Vehicle>(vehicles.OrderBy(s => s.id).Skip((pageIndex - 1) * pageSize).Take(pageSize));

            return vehicles;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("api/Vehicles/EditVehicle")]
        public HttpResponseMessage EditVehicle()
        {
            var httpRequest = HttpContext.Current.Request;

            TypeOfVehicle type = unitOfWork.TypesOfVehicle.Get(Int32.Parse(httpRequest["Type"]));

            Vehicle vehicle;
            
            try
            {
                vehicle = unitOfWork.Vehicles.Get(Int32.Parse(httpRequest["Id"]));
                vehicle.Manufactor = httpRequest["Manufactor"];
                vehicle.Model = httpRequest["Model"];
                vehicle.Description = httpRequest["Description"];
                vehicle.Year = Int32.Parse(httpRequest["Year"]);
                vehicle.PricePerHour = Decimal.Parse(httpRequest["PricePerHour"]);
            }
            catch
            {
                return null;
            }


            unitOfWork.Vehicles.Update(vehicle);
            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }


      //  [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("api/AddVehicle")]
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

            TypeOfVehicle type = unitOfWork.TypesOfVehicle.Get( Int32.Parse(httpRequest["Type"]) );

            Vehicle vehicle = new Vehicle { Images = imageName,
                Model = httpRequest["Model"],
                Manufactor = httpRequest["Manufactor"],
                Description = httpRequest["Description"],
                Year = Int32.Parse(httpRequest["Year"]),
                Type = type,
                PricePerHour= Decimal.Parse(httpRequest["Price"])
            };
            unitOfWork.Vehicles.Add(vehicle);
            Service s = unitOfWork.Services.Get(Int32.Parse(httpRequest["ServiceId"]));
            s.Vehicles.Add(vehicle);
            unitOfWork.Services.Update(s);
            unitOfWork.Complete();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(int id)
        {
            return db.Vehicles.Count(e => e.id == id) > 0;
        }
    }
}