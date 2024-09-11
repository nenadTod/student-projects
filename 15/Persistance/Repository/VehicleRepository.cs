using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {
        public VehicleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Vehicle> GetAll(int idService)
        {
            return RADBContext.Vehicles.Where<Vehicle>(v => v.ServiceId == idService).OrderBy(v => v.Model);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}