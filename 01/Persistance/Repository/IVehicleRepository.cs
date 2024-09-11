using RentApp.Models;
using RentApp.Models.Entities;
using System.Collections.Generic;

namespace RentApp.Persistance.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle, int>
    {
        IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize);
        IEnumerable<Vehicle> Search(string search);
        IEnumerable<Vehicle> Filter(Search search);
    }
}
