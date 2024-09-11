using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class AppUserRepository : Repository<AppUser, int>, IAppUserRepository
    {
        public AppUserRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<AppUser> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.AppUsers.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public AppUser GetUser(string userName)
        {
            return RADBContext.AppUsers.Where<AppUser>(u => u.Email == userName).FirstOrDefault();
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}