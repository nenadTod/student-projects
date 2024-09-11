using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class BranchOfficeRepository : Repository<BranchOffice, int>, IBranchOfficeRepository
    {
        protected RADBContext Context { get { return context as RADBContext; } }

        public BranchOfficeRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<BranchOffice> GetBranchOfficesForService(int serviceId)
        {
            return Context.Branches.Where(p => p.ServiceID == serviceId).ToList();
        }
    }
}