using RentApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
{
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            List<Service> services = new List<Service>(RADBContext.Services.Where(s => s.Activated == true));
             services = new List<Service>(services.OrderBy(s=>s.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize));
           
            return services;
            //return RADBContext.Services.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }

      
    }
}