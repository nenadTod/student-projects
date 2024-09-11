using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle, int>
    {
        IEnumerable<Vehicle> GetAllWithImages();
        IEnumerable<Vehicle> GetAllAvailableWithImages();
        IEnumerable<Vehicle> GetAllOfService(int serviceId);
        int Count();
        IEnumerable<Vehicle> GetVehiclePageWithImages(int pageIndex, int pageSize);
        IEnumerable<Vehicle> GetAvailableVehiclePageWithImages(int pageIndex, int pageSize);
    }
}
