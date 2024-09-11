using System;
using System.Collections.Generic;
using RentApp.Models.Entities;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation, int>
    {
        bool IsReserved(Vehicle vehicle, DateTime fromThisTime);
        List<Reservation> GetAllWithSecificVehicle(int vehicle);
    }
}