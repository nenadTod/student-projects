using RentApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
{
    public class BranchRepository : Repository<Branch, int>, IBranchRepository
    {
        public BranchRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Branch> GetAll(int pageIndex, int pageSize)
        {
            List<Branch> branches = new List<Branch>(RADBContext.Branches.OrderBy(b=>b.Address).Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return branches;
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}