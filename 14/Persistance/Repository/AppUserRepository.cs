using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class AppUserRepository:Repository<User,int>,IAppUserRepository
    {
        protected RADBContext rADBContext { get { return context as RADBContext; } }

        public AppUserRepository(DbContext context):base(context)
        {

        }

        public IEnumerable<User> GetAll(int pageIndex, int pageSize)
        {
            return rADBContext._Users.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}