using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ToVRepository : Repository<TypeOfVehicle, int>, IToVRepository
    {
        protected RADBContext Context { get { return context as RADBContext; } }
        public ToVRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<TypeOfVehicle> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vehicle> GetVehicles(int tovId)
        {
            return Context.Vehicles.Where(x => x.Type.Id == tovId).ToList();
        }
    }
}