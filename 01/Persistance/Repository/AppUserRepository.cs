using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
{
    public class AppUserRepository : Repository<AppUser, int>, IAppUserRepository
    {
        public AppUserRepository(DbContext context) : base(context)
        {
        }

        //public IEnumerable<Branch> GetAll(int pageIndex, int pageSize)
        //{
        //    return RADBContext.Branches.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //}

        protected RADBContext RADBContext { get { return context as RADBContext; } }

        public AppUser GetActiveUser(string username)
        {
            var user = RADBContext.Users.Where(u => u.Email == username).FirstOrDefault();
            var appUser = RADBContext.AppUsers.Where(u => u.Email == user.Email).FirstOrDefault();
            return appUser;
        }
    }
}