using RentApp.Persistance.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository Services { get; set; }
        IAppUserRepository AppUsers { get; set; }
        IBranchRepository Branches { get; set; }
        IRentRepository Rents { get; set; }
        ITypeOfVehicleRepository Types { get; set; }
        IVehicleRepository Vehicles { get; set; }

        ICommentRepository Comments { get; set; }

        int Complete();
    }
}
