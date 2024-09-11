using RentApp.Models.Entities;
using RentApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class RAIdentityUserRepository : Repository<RAIdentityUser, int>, IRAIdentityUserRepository
    {
        public RAIdentityUserRepository(DbContext context) : base(context)
        {
        }

        public  RAIdentityUser Get(string id)
        {
            return context.Set<RAIdentityUser>().Find(id);
        }

        public RAIdentityUser GetByAppUserId(int appUserId)
        {
            return context.Set<RAIdentityUser>().FirstOrDefault(u => u.AppUserId == appUserId);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}