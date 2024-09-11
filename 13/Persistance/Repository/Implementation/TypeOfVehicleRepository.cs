using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository.Implementation
{
    public class TypeOfVehicleRepository : Repository<TypeOfVehicle, int>, ITypeOfVehicleRepository
    {
        public TypeOfVehicleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<TypeOfVehicle> GetAll(int pageIndex, int pageSize)
        {
            return RADBContext.Types.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}