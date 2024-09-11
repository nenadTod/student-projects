using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {
        protected RADBContext Context { get { return context as RADBContext; } }
        public VehicleRepository(DbContext context) : base(context)
        {

        }

        protected RADBContext DemoContext { get { return context as RADBContext; } }

        public IEnumerable<Vehicle> GetVehiclesForService(int serviceId)
        {
            return Context.Vehicles.Where(x => x.Service.Id == serviceId).ToList();
        }

        public Service GetService(int serviceId)
        {
            return Context.Services.Find(serviceId);
        }

        public IEnumerable<Vehicle> GetByPrice(decimal price)
        {
            return Context.Vehicles.Where(x => x.PricePerHour <= price);
        }

        public Vehicle GetVehicle(int id)
        {
            return Context.Vehicles.Find(id);
        }
    }
}