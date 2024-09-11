using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IReservationRepository : IRepository<Reservation, int>
    {
        IEnumerable<Reservation> GetAllReservationsOfVehicle(int vehicleId);
        IEnumerable<Reservation> GetAllReservationsOfUser(int userId);
        IEnumerable<Reservation> GetAllUnpayedReservationsOfUser(int userId);
        IEnumerable<Reservation> GetAllReservationsOfUserWithBranchesAndService(int userId);
    }
}
