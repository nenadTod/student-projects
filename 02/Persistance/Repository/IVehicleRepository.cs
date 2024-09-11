using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle, int>
    {
        IEnumerable<Vehicle> GetVehiclesForService(int serviceId);
        Service GetService(int serviceId);

        Vehicle GetVehicle(int id);
        IEnumerable<Vehicle> GetByPrice(decimal price);
    }
}