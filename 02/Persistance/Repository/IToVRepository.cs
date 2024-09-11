using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IToVRepository : IRepository<TypeOfVehicle, int>
    {
        IEnumerable<TypeOfVehicle> GetAll(int pageIndex, int pageSize);
        IEnumerable<Vehicle> GetVehicles(int tovId);
    }
}
