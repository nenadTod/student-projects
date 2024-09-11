using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class RolesRepository : Repository<IdentityRole, int>, IRolesRepository
    {
        public RolesRepository(DbContext context) : base(context)
        {
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}