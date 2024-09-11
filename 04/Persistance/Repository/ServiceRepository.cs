using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(DbContext context) : base(context)
        {
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Services.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public Service GetWithItemsAndPricelists(int serviceId)
        {
            return context.Set<Service>().Include("Pricelists.Items").First(s => s.Id == serviceId);
        }


        public Service GetWithPricelists(int serviceId)
        {
            return context.Set<Service>().Include("Pricelists").First(s => s.Id == serviceId);
        }

        public Service GetServiceWithVehicles(int serviceId)
        {
            return context.Set<Service>().Include("Vehicles.Images").First(s => s.Id == serviceId);
        }

        public Service GetServiceWithVehiclesAndPricelists(int serviceId)
        {
            return context.Set<Service>().Include("Vehicles.Images").Include("Pricelists.Items").First(s => s.Id == serviceId);
        }
    }
}