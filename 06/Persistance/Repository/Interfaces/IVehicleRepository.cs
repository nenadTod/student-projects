using System.Collections.Generic;
using RentApp.Models.Entities;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle, int>
    {
        IEnumerable<Vehicle> GetRange(int pageIndex, int pageSize);
        IEnumerable<Vehicle> GetVehiclesFromServiceId(int serviceId);
        void UploadImage(string image, int id);
    }
}