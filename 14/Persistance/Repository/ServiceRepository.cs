using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ServiceRepository:Repository<Service, int>,IServiceRepository
    {
        protected RADBContext rADBContext { get { return context as RADBContext; } }

        public ServiceRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            return rADBContext.Services.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
