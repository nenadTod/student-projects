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
        IServiceRepository Services { get; set; }
        IAppUserRepositroy AppUsers { get; set; }
        IVehicleRepository Vehicles { get; set; }
        IBranchOfficeRepository BranchOffice { get; set; }
        IReservationRepository Reservations { get; set; }
        ICommentRepository Comments { get; set; }

        int Complete();
    }
}
