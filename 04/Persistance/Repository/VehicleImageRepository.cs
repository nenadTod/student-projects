using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class VehicleImageRepository : Repository<VehicleImage, int>, IVehicleImageRepository
    {
        public VehicleImageRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<VehicleImage> GetAllOfVehicle(int vehicleId)
        {
            return RADBContext.VehicleImages.Where(vi => vi.VehicleImageVehicleId == vehicleId);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }

}