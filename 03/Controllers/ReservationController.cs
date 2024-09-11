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

        private IUnitOfWork db;

        public ReservationController(IUnitOfWork context)
        {
            db = context;
        }

        // GET: api/Services
        public IEnumerable<Reservation> GetReservations()
        {
            return db.Reservations.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetService(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ReservationID)
            {
                return BadRequest();
            }

            try
            {
                db.Reservations.Update(reservation);
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(ReservationBinding reservationBinding)
        {
            Reservation reservation = new Reservation();
            reservation.ClientID = reservationBinding.ClientID;
            reservation.VehicleID = reservationBinding.VehicleID;
            reservation.GetBranchId = reservationBinding.GetBranchId;
            reservation.ReturnBranchId = reservationBinding.ReturnBranchId;
            string getDateTime = string.Concat(reservationBinding.GetDate + " ", reservationBinding.GetTime);
            DateTime dt = DateTime.Parse(getDateTime);
            reservation.GetVehicleDate = dt;

            string returnDateTime = string.Concat(reservationBinding.ReturnDate + " ", reservationBinding.ReturnTime);
            DateTime dt1 = DateTime.Parse(returnDateTime);
            reservation.ReturnVehicleDate = dt1;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Reservations.Add(reservation);
            db.Complete();

            return CreatedAtRoute("DefaultApi", new { id = reservation.ReservationID }, reservation);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Reservation))]
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
            return db.Reservations.Get(id) != null;
        }

    }
}
