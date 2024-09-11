using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RentApp.Persistance.Repository
{
    public class VehicleTypeRepository : Repository<VehicleType, int>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<VehicleType> GetAll(int pageIndex, int pageSize)
        {
            return Context.VehicleTypes.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext Context { get { return RADBContext.Create(); } }
    }
}