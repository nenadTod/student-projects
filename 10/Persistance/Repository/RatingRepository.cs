using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class RatingRepository : Repository<Rating, int>, IRatingRepository
    {
        public RatingRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Rating> GetAll(int pageIndex, int pageSize)
        {
            return RadbContext.Ratings.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RadbContext { get { return context as RADBContext; } }
    }
}