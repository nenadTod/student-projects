using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository.Interface
{
    public interface IBranchRepository : IRepository<Branch, int>
    {
        IEnumerable<Branch> GetAll(int pageIndex, int pageSize);
    }
}
