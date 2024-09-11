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
        IAppUserRepository AppUsers { get; set; }

        IBranchRepository Branches { get; set; }
        IRentRepository Rents { get; set; }
        ITypeOfVehicleRepository TypesOfVehicle { get; set; }
        IVehicleRepository Vehicles { get; set; }
        ICommentRepository Comments { get; set; }
        int Complete();
    }
}
