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
        IAppUserRepository AppUsers { get; set; }
        IBranchRepository Branches { get; set; }
        IRentRepository Rents { get; set; }
        IServicesRepository Services { get; set; }
        IVehicleRepository Vehicles { get; set; }
        ITypeOfVehicleRepository TypesOfVehicle { get; set; }
        ICommentRepository Comments { get; set; }
        IRatingRepository Ratings { get; set; }
        ITransactionRepository Transactions { get; set; }

        int Complete();
    }
}
