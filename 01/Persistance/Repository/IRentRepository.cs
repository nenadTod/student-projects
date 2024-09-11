using RentApp.Models.Entities;
using System.Collections.Generic;

namespace RentApp.Persistance.Repository
{
    public interface IRentRepository : IRepository<Rent, int>
    {
        IEnumerable<Rent> GetAll(int pageIndex, int pageSize);
    }
}
