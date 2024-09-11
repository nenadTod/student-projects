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
        protected RADBContext Context { get { return context as RADBContext; } }
        public BranchRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<Branch> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Service GetService(int serviceId)
        {
            return Context.Services.Find(serviceId);
        }

        public Branch GetBranch(string branch)
        {
            return Context.Branches.Find(branch);
        }
    }
}