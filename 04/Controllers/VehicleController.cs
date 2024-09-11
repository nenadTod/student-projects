using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Helper;
using RentApp.Models;
using RentApp.Models.Entities;
using RentApp.Persistance.Repository;
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
    public class VehicleController : ApiController
    {
        private IUnitOfWork db;

        public VehicleController(IUnitOfWork context)
        {
            db = context;
        }
        
        // GET: api/Services
        [AllowAnonymous]
        public IEnumerable<VehicleDTO> GetVehicles()
        {
            
            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();
            IEnumerable<Vehicle> vehicles  = db.Vehicles.GetAll();
            foreach (Vehicle vehicle in vehicles)
            {
                Service service = db.Services.GetWithItemsAndPricelists(vehicle.VehicleServiceId);
                VehicleDTO vehicleDTO = new VehicleDTO(vehicle);
                if (service.Pricelists.Count > 0)
                {
                    Pricelist actualPriceList = service.Pricelists[0];
                    foreach (Pricelist pricelist in service.Pricelists.Where(p => p.BeginTime <= DateTime.Now.Date))
                    {
                        if (pricelist.EndTime > actualPriceList.EndTime)
                        {
                            actualPriceList = pricelist;
                        }
                    }
                    try
                    {
                        Item item = actualPriceList.Items.First(i => i.ItemVehicleId == vehicle.Id);
                        vehicleDTO.PricePerHour = item.Price;
                    }
                    catch (Exception e)
                    {
                        vehicleDTO.PricePerHour = 0;
                    }
                }
                else
                {
                    vehicleDTO.PricePerHour = 0;
                }
                vehiclesDTO.Add(vehicleDTO);
            }

            return vehiclesDTO;
        }


        [HttpGet]
        [Route("api/Vehicles/GetVehiclesPage/{pageIndex}")]
        [AllowAnonymous]
        public GetVehiclesPageResult GetVehiclesPage(int pageIndex)
        {
            string name = User.Identity.Name;
            RAIdentityUser user = db.Users.Get(name);
            IdentityRole managerRole = db.Roles.GetAll().FirstOrDefault(role => role.Name == "Manager");
            bool isManager = false;
            if (user != null)
            {
                foreach (IdentityUserRole userRole in user.Roles)
                {
                    if (userRole.RoleId == managerRole.Id)
                    {
                        isManager = true;
                        break;
                    }
                }
            }
            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();
            IEnumerable<Vehicle> vehicles;
            if (isManager)
            {
                vehicles = db.Vehicles.GetVehiclePageWithImages(pageIndex, 4);
            }
            else
            {
                vehicles = db.Vehicles.GetAvailableVehiclePageWithImages(pageIndex, 4);
            }

            foreach (Vehicle vehicle in vehicles)
            {
                Service service = db.Services.GetWithItemsAndPricelists(vehicle.VehicleServiceId);
                VehicleDTO vehicleDTO = new VehicleDTO(vehicle);
                if (service.Pricelists.Count > 0)
                {
                    Pricelist actualPriceList = service.Pricelists[0];
                    foreach (Pricelist pricelist in service.Pricelists.Where(p => p.BeginTime <= DateTime.Now.Date))
                    {
                        if (pricelist.EndTime > actualPriceList.EndTime)
                        {
                            actualPriceList = pricelist;
                        }
                    }
                    try
                    {
                        Item item = actualPriceList.Items.First(i => i.ItemVehicleId == vehicle.Id);
                        vehicleDTO.PricePerHour = item.Price;
                    }
                    catch (Exception e)
                    {
                        vehicleDTO.PricePerHour = 0;
                    }
                }
                else
                {
                    vehicleDTO.PricePerHour = 0;
                }
                vehiclesDTO.Add(vehicleDTO);
            }
            GetVehiclesPageResult result = new GetVehiclesPageResult();
            result.Vehicles = vehiclesDTO;
            result.Count = db.Vehicles.Count();
            return result;
        }

        [HttpPost]
        [Route("api/Vehicles/GetVehiclesPageFiltered/{pageIndex}")]
        [AllowAnonymous]
        public GetVehiclesPageResult GetVehiclesPageFiltered(int pageIndex, VehicleFilterModel vehicleFilter)
        {
            string name = User.Identity.Name;
            RAIdentityUser user = db.Users.Get(name);
            IdentityRole managerRole = db.Roles.GetAll().FirstOrDefault(role => role.Name == "Manager");
            bool isManager = false;
            if (user != null)
            {
                foreach (IdentityUserRole userRole in user.Roles)
                {
                    if (userRole.RoleId == managerRole.Id)
                    {
                        isManager = true;
                        break;
                    }
                }
            }
            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();
            SearchFilter searchFilter = new SearchFilter(vehicleFilter);
            IEnumerable<Vehicle> vehicles;
            if (isManager)
            {
                vehicles = db.Vehicles.GetAllWithImages();
            }
            else
            {
                vehicles = db.Vehicles.GetAllAvailableWithImages();
            }

            foreach (Vehicle vehicle in vehicles)
            {
                if (!searchFilter.CheckVehicle(vehicle))
                {
                    continue;
                }
                VehicleDTO vehicleDTO = new VehicleDTO(vehicle);
                Service service = db.Services.GetWithItemsAndPricelists(vehicle.VehicleServiceId);
                if (service.Pricelists.Count > 0)
                {
                    Pricelist actualPriceList = service.Pricelists[0];
                    foreach (Pricelist pricelist in service.Pricelists.Where(p => p.BeginTime <= DateTime.Now.Date))
                    {
                        if (pricelist.EndTime > actualPriceList.EndTime)
                        {
                            actualPriceList = pricelist;
                        }
                    }
                    try
                    {
                        Item item = actualPriceList.Items.First(i => i.ItemVehicleId == vehicle.Id);
                        vehicleDTO.PricePerHour = item.Price;
                    }
                    catch (Exception e)
                    {
                        vehicleDTO.PricePerHour = 0;
                    }
                }
                else
                {
                    vehicleDTO.PricePerHour = 0;
                }

                if (searchFilter.CheckVehiclePrice(vehicleDTO))
                {
                    vehiclesDTO.Add(vehicleDTO);
                }
            }

            GetVehiclesPageResult result = new GetVehiclesPageResult();
            result.Count = vehiclesDTO.Count();
            result.Vehicles = vehiclesDTO.OrderBy(v => v.Id).Skip((pageIndex - 1) * 4).Take(4);
            return result;
        }

        [HttpGet]
        [Route("api/Vehicles/GetVehiclesCount")]
        [AllowAnonymous]
        public int GetVehiclesCount()
        {
            return db.Vehicles.Count();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // GET: api/Services/5
        [ResponseType(typeof(int))]
        [AllowAnonymous]
        public IHttpActionResult GetVehiclePrice(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.GetAll().First(u => u.UserName == username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);
                        
            Service service = db.Services.Get(vehicle.VehicleServiceId);
            if (service.ServiceManagerId != appUser.Id)
            {
                return BadRequest("You are not authorized.");
            }

            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            db.Vehicles.Update(vehicle);

            try
            {
                db.Complete();
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

        [HttpPut]
        [Route("api/Vehicle/ToggleVehicleAvailability/{vehicleId}")]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult ToggleVehicleAvailability(int vehicleId)
        {
            Vehicle vehicle = db.Vehicles.Get(vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            Service service = db.Services.Get(vehicle.VehicleServiceId);
            if (service.ServiceManagerId != appUser.Id)
            {
                return BadRequest("You are not authorized.");
            }

            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            vehicle.IsAvailable = !vehicle.IsAvailable;
            db.Vehicles.Update(vehicle);
            db.Complete();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Services
        [ResponseType(typeof(Vehicle))]
        [Authorize(Roles = "Manager")]
        public IHttpActionResult PostVehicle(VehicleDTO vehicleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);
            
            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            Vehicle vehicle = new Vehicle();
            vehicle.Description = vehicleDTO.Description;
            vehicle.Id = vehicleDTO.Id;
            vehicle.IsAvailable = vehicleDTO.IsAvailable;
            vehicle.Manufacturer = vehicleDTO.Manufacturer;
            vehicle.Model = vehicleDTO.Model;
            vehicle.TypeId = vehicleDTO.TypeId;
            vehicle.VehicleServiceId = vehicleDTO.VehicleServiceId;
            vehicle.YearOfProduction = vehicleDTO.YearOfProduction;

            Service service = db.Services.GetWithPricelists(vehicle.VehicleServiceId);
            if (!service.IsConfirmed)
            {
                return BadRequest("Service is not confirmed yet.");
            }

            Item item = new Item();
            item.ItemVehicleId = vehicle.Id;
            
            Pricelist actualPricelist = service.Pricelists[0];
            foreach (Pricelist pricelist in service.Pricelists.Where(p => p.BeginTime <= DateTime.Now.Date))
            {
                if (pricelist.EndTime > actualPricelist.EndTime)
                {
                    actualPricelist = pricelist;
                }
            }
            item.ItemPriceListId = actualPricelist.Id;
            item.Price = vehicleDTO.PricePerHour;
            db.Vehicles.Add(vehicle);
            db.Items.Add(item);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = vehicle.Id }, vehicle);
        }


        

        // DELETE: api/Services/5
        [ResponseType(typeof(Vehicle))]
        [Authorize(Roles ="Manager")]
        public IHttpActionResult DeleteVehicle(int id)
        {
            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            Service service = db.Services.Get(vehicle.VehicleServiceId);
            if (service.ServiceManagerId != appUser.Id)
            {
                return BadRequest("You are not authorized.");
            }

            if (appUser.IsManagerAllowed==false)
            {
                return BadRequest("You are not allowed.");
            }

            db.Vehicles.Remove(vehicle);

            List<VehicleImage> images = db.VehicleImages.GetAll().Where(vi => vi.VehicleImageVehicleId == id).ToList();

            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Content/Images/VehicleImages/") + id);
            directory.Delete(true);
            db.Vehicles.Remove(vehicle);

            db.Complete();
            
            return Ok(vehicle);
        }

        
        //[Route("api/Vehicle/DeleteVehicleWithServiceId")]
        //[HttpDelete]
        //[ResponseType(typeof(Vehicle))]
        //public IHttpActionResult DeleteVehicleWithServiceId(int serviceId)
        //{
        //    List<Vehicle> vehicles = db.Vehicles.GetAll().Where(t=>t.VehicleServiceId==serviceId).ToList();
        //    if (vehicles == null)
        //    {
        //        return NotFound();
        //    }

        //    foreach(Vehicle vehicle in vehicles)
        //    {
        //        db.Vehicles.Remove(vehicle);
        //        foreach (VehicleImage image in vehicle.Images)
        //        {
        //            string destinationFilePath = HttpContext.Current.Server.MapPath("~/");
        //            destinationFilePath += image.ImagePath;
        //            if (File.Exists(destinationFilePath))
        //            {
        //                File.Delete(destinationFilePath);
        //            }
        //        }
        //    }
            
        //    db.Complete();

        //    return Ok();
        //}


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
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
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
