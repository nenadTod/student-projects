using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {

        protected RADBContext Context { get { return context as RADBContext; } }

        public VehicleRepository(DbContext context) : base(context)
        {
        }
        public IEnumerable<Vehicle> GetVehiclesForService(int serviceId)
        {
            return Context.Vehicles.Where(p => p.ServiceId == serviceId).ToList();
        }
    }
}