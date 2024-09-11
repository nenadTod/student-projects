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

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            return Context.Services.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return new List<Service>() { new Service() { Authorized = true, Name = "Audi", Email = "Audi@gmail.com", Id = 1, BranchOffices = new List<BranchOffice>(), Description = "We are Audi, buy our cars" } };
        }

        // Vraca sve servise koji cekaju odobrenje
        public IEnumerable<Service> GetAwaitingServices()
        {
            return Context.Services.Where(s => s.Authorized == false).ToList();
        }

        protected RADBContext Context { get { return RADBContext.Create(); } }
    }
}