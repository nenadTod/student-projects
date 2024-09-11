using RentApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
{
    public class TypeOfVehicleRepository : Repository<TypeOfVehicle, int>, ITypeOfVehicleRepository
    {
        public TypeOfVehicleRepository(DbContext context) : base(context)
        {
        }

        //public IEnumerable<Branch> GetAll(int pageIndex, int pageSize)
        //{
        //    return RADBContext.Branches.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //}

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}