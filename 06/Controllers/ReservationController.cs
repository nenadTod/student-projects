using System.Collections.Generic;
using System.Web.Http;
using RentApp.Models;
using RentApp.Models.Entities;
using RentApp.Persistance.UnitOfWork;

namespace RentApp.Controllers
{
    public class ReservationController : ApiController
    {
        private IUnitOfWork _uow;

        public ReservationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("reservations/getall")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var retVal = _uow.Reservations.GetAll();
            return Ok(retVal);
        }

        [Route("reservations/add")]
        [HttpPut]
        public IHttpActionResult Add(Reservation reservation)
        {
            List<Reservation> reservations = _uow.Reservations.GetAllWithSecificVehicle(reservation.VehicleId);
            bool reserved = false;
            List<ReservationDateModel> reservedDates = new List<ReservationDateModel>();

            foreach(var r in reservations){
                if(r.TimeFrom >= reservation.TimeFrom && r.TimeFrom <= reservation.TimeTo){
                    reservedDates.Add(new ReservationDateModel() { Start = r.TimeFrom, End = r.TimeTo });
                    reserved = true;
                }
                else if (r.TimeTo >= reservation.TimeFrom && r.TimeTo <= reservation.TimeTo)
                {
                    reservedDates.Add(new ReservationDateModel() { Start = r.TimeFrom, End = r.TimeTo });
                    reserved = true;
                }
                else
                {
                    reservedDates.Add(new ReservationDateModel() { Start = r.TimeFrom, End = r.TimeTo });
                }
            }
            if (reserved)
            {
                return Ok(reservedDates);
            }

            _uow.Reservations.Add(reservation);
            _uow.Complete();
            return Ok();
        }

        [Route("reservation/remove")]
        [HttpDelete]
        public IHttpActionResult Remove(Reservation reservation)
        {
            _uow.Reservations.Remove(reservation);
            _uow.Complete();
            return Ok();
        }

        [Route("reservation/removeId")]
        [HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            _uow.Reservations.Remove(_uow.Reservations.Get(id));
            _uow.Complete();
            return Ok();
        }

        [Route("reservation/update")]
        [HttpPut]
        public IHttpActionResult Update(Reservation reservation)
        {
            _uow.Reservations.Update(reservation);
            _uow.Complete();
            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            _uow.Dispose();
            base.Dispose(disposing);
        }


    }
}