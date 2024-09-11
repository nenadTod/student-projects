using RentApp.Models;
using RentApp.Models.DTO;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RentApp.Controllers
{
    public class ReservationController : ApiController
    {
        private static object reservationLockObject = new object();
        private IUnitOfWork db;

        public ReservationController(IUnitOfWork context)
        {
            db = context;
        }

        [Authorize(Roles = "AppUser, Manager, Admin")]
        // GET: api/Services
        public IEnumerable<Reservation> GetReservations()
        {
            return db.Reservations.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Reservation))]
        [Authorize(Roles = "AppUser, Manager, Admin")]
        public IHttpActionResult GetReservation(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpGet]
        [Route("api/Reservations/GetReservationsOfVehicle/{vehicleId}")]
        [Authorize(Roles = "AppUser, Manager, Admin")]
        public IEnumerable<Reservation> GetReservationsOfVehicle(int vehicleId)
        {
            return db.Reservations.GetAllReservationsOfVehicle(vehicleId);
        }

        [HttpGet]
        [Route("api/Reservations/GetReservationsOfUser/{userId}")]
        [Authorize(Roles = "AppUser, Manager, Admin")]
        public GetReservationsPageResult GetReservationsOfUser(int userId)
        {
            List<Reservation> reservations = db.Reservations.GetAllUnpayedReservationsOfUser(userId).ToList();
            List<ReservationDTO> reservationsDTO = new List<ReservationDTO>();
            double priceCount = 0;

            foreach (Reservation reservation in reservations)
            {
                ReservationDTO rdto = new ReservationDTO(reservation);
                double days = (reservation.EndTime - reservation.BeginTime).TotalDays;
                Vehicle vehicle = db.Vehicles.Get(reservation.ReservedVehicleId);
                double pricePerHour = 0;

                Service service = db.Services.GetWithItemsAndPricelists(vehicle.VehicleServiceId);

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
                    pricePerHour = item.Price;
                }
                catch (Exception e)
                {
                    pricePerHour = 0;
                }

                rdto.PriceToPay = days * 24 * pricePerHour;

                priceCount += rdto.PriceToPay;

                reservationsDTO.Add(rdto);
            }

            GetReservationsPageResult result = new GetReservationsPageResult();
            result.Reservations = reservationsDTO;
            result.PriceToPay = priceCount;

            return result;
        }

        [HttpPut]
        [Route("api/Reservations/PayedReservationsOfUser/{userId}/{paymentID}")]
        [Authorize(Roles = "AppUser")]
        public IHttpActionResult PayedReservationsOfUser(int userId, string paymentID)
        {
            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            if(userId!=appUser.Id)
            {
                return BadRequest();
            }

            List<Reservation> reservations = db.Reservations.GetAllUnpayedReservationsOfUser(userId).ToList();
            
            foreach(Reservation reservation in reservations)
            {
     
                reservation.Payed = true;
                reservation.PaymentId = paymentID;
                db.Reservations.Update(reservation);
            }

            db.Complete();

            return StatusCode(HttpStatusCode.NoContent);
        }


        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "AppUser")]
        public IHttpActionResult PutReservation(int id, TransactionElement element)
        {
            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            if (element.userId != appUser.Id)
            {
                return BadRequest();
            }

            List<Reservation> reservations = db.Reservations.GetAllUnpayedReservationsOfUser(element.userId).ToList();

            foreach (Reservation reservation in reservations)
            {

                reservation.Payed = true;
                reservation.PaymentId = element.paymentId;
                db.Reservations.Update(reservation);
            }

            db.Complete();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Services
        [ResponseType(typeof(Reservation))]
        [Authorize(Roles = "AppUser")]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reservation.BeginTime > reservation.EndTime)
            {
                return BadRequest("Begin time need to be before end time.");
            }

            if (reservation.BeginTime < DateTime.Now.Date || reservation.EndTime < DateTime.Now.Date)
            {
                return BadRequest("Begin and end time should be after today.");
            }

            if (reservation.BranchDropOffId == -1 || reservation.BranchTakeId == -1)
            {
                return BadRequest("Choose valid branches.");

            }

            string username = User.Identity.Name;
            RAIdentityUser RAUser = db.Users.Get(username);
            AppUser appUser = db.AppUsers.Get(RAUser.AppUserId);

            if(appUser.IsUserConfirmed==false)
            {
                return BadRequest("You are not confirmed.");
            }

            Vehicle vehicle = db.Vehicles.Get(reservation.ReservedVehicleId);
            if (vehicle.IsAvailable==false)
            {
                return BadRequest("Vehicle not available.");
            }

            lock (reservationLockObject)
            {
                List<Reservation> reservations = db.Reservations.GetAllReservationsOfVehicle(reservation.ReservedVehicleId).ToList();
                foreach (Reservation r in reservations)
                {
                    if (reservation.BeginTime >= r.BeginTime && reservation.BeginTime <= r.EndTime)
                    {
                        return BadRequest("Vehicle is reserved in this period, try different time period.");
                    }
                    if (reservation.EndTime >= r.BeginTime && reservation.EndTime <= r.EndTime)
                    {
                        return BadRequest("Vehicle is reserved in this period, try different time period.");
                    }

                    if (reservation.BeginTime < r.BeginTime && reservation.EndTime > r.EndTime)
                    {
                        return BadRequest("Vehicle is reserved in this period, try different time period.");
                    }
                }
                reservation.Payed = false;
                db.Reservations.Add(reservation);
                db.Complete();
            }
            return CreatedAtRoute("DefaultApi", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Reservation))]
        [Authorize(Roles = "AppUser, Manager, Admin")]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservations.Remove(reservation);
            db.Complete();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            if (reservation == null)
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
