using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ItemRepository : Repository<Item, int>, IItemRepository
    {
        public ItemRepository(DbContext context) : base(context)
        {
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }

    }
}