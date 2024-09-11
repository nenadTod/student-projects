using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class ItemRepository : Repository<Item, int>, IItemRepository
    {
        public ItemRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Item> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Items.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}