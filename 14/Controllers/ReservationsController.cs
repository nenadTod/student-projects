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

namespace RentApp.Controllers
{
    [RoutePrefix("api/Reservations")]
    public class ReservationsController : ApiController
    {
        //private RADBContext db = new RADBContext();
        private IUnitOfWork unitOfWork;

        public ReservationsController(IUnitOfWork context)
        {
            this.unitOfWork = context;
        }


        // GET: api/Reservations
        [Route("FromUser/{id}")]
        public IEnumerable<Reservation> GetReservations(Guid id)
        {
            RADBContext context = new RADBContext();
            List<Guid> reservations = new List<Guid>();
            return context._Users
                .Include(x => x.Reservations)
                .Include(x => x.Reservations.Select(q=>q.Service))
                .Include(x => x.Reservations.Select(q => q.StartBranchOffice))
                .Include(x => x.Reservations.Select(q => q.Vehicle))
                .Include(x => x.Reservations.Select(q => q.EndBranchOffice))
                .FirstOrDefault(x => x.Id.CompareTo(id) == 0).Reservations;
           
            
            
        }

        [Route("ForUser/{id}")]
        public int PostReservationForUser(Guid id,Reservation reservation)
        {
            RADBContext context = new RADBContext();
            User u = context._Users.Include(x => x.Reservations).FirstOrDefault(x => x.Id.CompareTo(id) == 0);
            if (u == default(User))
            {
                return 1;
            }
            u.Reservations.Add(reservation);
            context.SaveChanges();
            return 0;
        }



        // GET: api/Reservations/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetReservation(Guid id)
        {
            Reservation reservation = unitOfWork.Reservations.Find(r=> r.Id == id).FirstOrDefault();
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/Reservations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(Guid id, Reservation reservation)
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

        // POST: api/Reservations
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.Reservations.Add(reservation);

            try
            {
                unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                if (ReservationExists(reservation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(Guid id)
        {
            Reservation reservation = unitOfWork.Reservations.Find(r=> r.Id == id).FirstOrDefault();
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

        private bool ReservationExists(Guid? id)
        {
            return unitOfWork.Reservations.Find(r => r.Id == id).FirstOrDefault() != null;
        }
    }
}