using System.Collections.Generic;
using RentApp.Models.Entities;

namespace RentApp.Persistance.Repository.Interfaces
{
    public interface IServiceRepository : IRepository<Service, int>
    {
        IEnumerable<Service> GetAllNonApproved();
        void AddNewVehicle(Vehicle vehicle, int serviceId);
        bool RemoveVehicle(Vehicle vehicle, Service service);
        Service GetDetails(int id);
        IEnumerable<Service> GetServiceById(int id);
        IEnumerable<Service> GetAllApprovedServices();
        void ApproveService(Service service);
        void UploadImage(string base64String, int id);
        IEnumerable<Service> GetServicesForManager(int id);
        void AddNewBranchOffice(int id, BranchOffice office);
        float Rate(int serviceId, float rate);

    }
}