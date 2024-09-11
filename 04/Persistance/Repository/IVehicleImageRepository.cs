using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IVehicleImageRepository : IRepository<VehicleImage, int>
    {
        IEnumerable<VehicleImage> GetAllOfVehicle(int vehicleId);
    }
}
