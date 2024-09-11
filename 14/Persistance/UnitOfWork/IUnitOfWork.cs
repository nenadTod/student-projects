using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository AppUsers { get; set; }
        IBranchOfficeRepository BranchOffices { get; set; }
        IReservationRepository Reservations { get; set; }
        IVehicleRepository Vehicles { get; set; }
        IServiceRepository Services { get; set; }

        int Complete();
    }
}
