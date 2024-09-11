using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ReservationRepository : Repository<Reservation, int>, IReservationRepository
    {
        public ReservationRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Reservation> GetAllReservationsOfVehicle(int vehicleId)
        {
            return RADBContext.Reservations.Where( r => r.ReservedVehicleId == vehicleId);
        }

        public IEnumerable<Reservation> GetAllReservationsOfUserWithBranchesAndService(int userId)
        {
            return context.Set<Reservation>().Include("BranchTake.BranchService").Where(r => r.UserId == userId);
        }

        public IEnumerable<Reservation> GetAllReservationsOfUser(int userId)
        {
            return RADBContext.Reservations.Where(r => r.UserId == userId);
        }

        public IEnumerable<Reservation> GetAllUnpayedReservationsOfUser(int userId)
        {
            return RADBContext.Reservations.Where(r => r.UserId == userId && r.Payed == false);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}