using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.Repository
{
    public interface IServiceRepository : IRepository<Service, int>
    {
        IEnumerable<Service> GetAll(int pageIndex, int pageSize);

        IEnumerable<Vehicle> GetVehicles(int serviceId);
        IEnumerable<Branch> GetBranches(int serviceId);
        IEnumerable<Comment> GetComments(int serviceId);
        void DeleteVehicles(int serviceId);
        void DeleteBranches(int serviceId);
        void DeleteComments(int serviceId);
        IEnumerable<Service> GetActiveServices();
        IEnumerable<Service> GetDeactiveServices();
    }
}