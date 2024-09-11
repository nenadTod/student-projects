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
        private readonly IUnitOfWork unitOfWork;

        public ReservationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Services
        public IEnumerable<Reservation> GetReservations()
        {
            return unitOfWork.Reservations.GetAll();
        }

        // GET: api/Services/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetReservation(int VehicleId, DateTime DateRezervation, DateTime ReturnDate)
        {
            bool reservation = unitOfWork.Reservations.GetReservation(VehicleId, DateRezervation, ReturnDate);
            if (!reservation)
            {
                return NotFound();
            }
            else
            {
                return Ok(reservation);
            }

        }

        [Authorize(Roles = "Admin, Manager, AppUser")]
        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.Reservations.Update(reservation);
                unitOfWork.Complete();
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

        [Authorize(Roles = "Admin, Manager, AppUser")]
        // POST: api/Services
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Reservations.Add(reservation);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = unitOfWork.Reservations.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }

            unitOfWork.Reservations.Remove(reservation);
            unitOfWork.Complete();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return unitOfWork.Reservations.Get(id) != null;
        }
    }
}
