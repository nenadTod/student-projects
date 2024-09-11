using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class PricelistRepository : Repository<PriceList, int>, IPricelistRepository
    {
        public PricelistRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<PriceList> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Pricelists.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}