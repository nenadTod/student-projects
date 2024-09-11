using RentApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository Services { get; set; }
        IBranchOfficeRepository BranchOffices { get; set; }
        IRentRepository Rents { get; set; }
        IVehicleRepository Vehicles { get; set; }
        IVehicleTypeRepository VehicleTypes { get; set; }
        IAppUserRepository AppUserRepository { get; set; }
        IReviewRepository Reviews { get; set; }
        int Complete();
    }
}
