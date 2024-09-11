using RentApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
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

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}