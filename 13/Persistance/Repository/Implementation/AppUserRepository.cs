using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository.Implementation
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

        public AppUser GetFromUsername(string username)
        {
            return RADBContext.AppUsers.FirstOrDefault( a => a.Username.Equals(username));

        }


        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}