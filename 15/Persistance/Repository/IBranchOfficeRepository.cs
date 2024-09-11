using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IBranchOfficeRepository : IRepository<BranchOffice, int>
    {
        IEnumerable<BranchOffice> GetAll(int idService);
        BranchOffice GetBranch(int idService, double lat, double lgt);
    }
}
