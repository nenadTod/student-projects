using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class ReservationRepository : Repository<Reservation, int>, IReservationRepository
    {
        public ReservationRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Reservation> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Reservations.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public bool GetReservation(int VehicleId, DateTime DateRezervation, DateTime ReturnDate)
        {
            var list = RADBContext.Reservations.Where<Reservation>(v => v.VehicleId == VehicleId).OrderBy(v => v.DateRezervation);

            foreach(Reservation r in list)
            {
                if(DateRezervation == r.DateRezervation)
                {
                    return false;
                }
                else if (ReturnDate == r.ReturnDate)
                {
                    return false;
                }
                else if (DateRezervation < r.DateRezervation && ReturnDate >= r.DateRezervation)
                {
                    return false;
                }
                else if (DateRezervation <= r.ReturnDate && DateRezervation >= r.DateRezervation)
                {
                    return false;
                }
            }

            return true;
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}