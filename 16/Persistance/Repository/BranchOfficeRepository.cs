using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq.Expressions;

namespace RentApp.Persistance.Repository
{
    public class BranchOfficeRepository : Repository<BranchOffice, int>, IBranchOfficeRepository
    {
        public BranchOfficeRepository(DbContext context) : base(context)
        {
        }
        public IEnumerable<BranchOffice> GetAll(int pageIndex, int pageSize)
        {
            return Context.BranchOffices.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext Context { get { return RADBContext.Create(); } }
    }
}