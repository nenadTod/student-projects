using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RentApp.Persistance;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {
        protected RADBContext rADBContext { get { return context as RADBContext; } }

        public VehicleRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize)
        {
            return rADBContext.Vehicles.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}