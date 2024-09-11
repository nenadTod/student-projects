using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository.Implementation
{
    public class RentRepository : Repository<Rent, int>, IRentRepository
    {
        public RentRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Rent> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Rents.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        }

        public IEnumerable<Rent> GetAllRents()
        {
            return RADBContext.Rents.Include(i => i.Vehicle).Include(a => a.RetBranch).Include(sa => sa.GetBranch).ToList();

        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}