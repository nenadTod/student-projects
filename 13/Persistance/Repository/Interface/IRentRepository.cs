using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository.Interface
{
    public interface IRentRepository : IRepository<Rent, int>
    {
        IEnumerable<Rent> GetAll(int pageIndex, int pageSize);

        IEnumerable<Rent> GetAllRents();
    }
}
