using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class BranchRepository : Repository<Branch, int>, IBranchRepository
    {
        public BranchRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Branch>  GetAllOfService(int serviceId)
        {
            return context.Set<Branch>().Where(b => b.BranchServiceId == serviceId);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
               
    }
}