using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ServicesRepository : Repository<Service, int>, IServicesRepository
    {
        public ServicesRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            RADBContext db = new RADBContext();
            List<Service> services = new List<Service>(db.Services);
            services = new List<Service>(services.OrderBy(s=>s.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize));

            return services;
        }

        protected RADBContext RadbContext { get { return context as RADBContext; } }
    }
}