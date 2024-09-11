using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ReviewRepository : Repository<Review, int>, IReviewRepository
    {

        public ReviewRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Review> GetAll(int pageIndex, int pageSize)
        {
            return new List<Review>();
        }

        protected RADBContext Context { get { return RADBContext.Create(); } }
    }
}