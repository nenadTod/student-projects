using RentApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Persistance.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IServiceRepository Services { get; set; }
        IUserRepository Users { get; set; }
        IBranchRepository Branches { get; set; }
        IToVRepository TypesOfVehicle { get; set; }
        IRentRepository Rents { get; set; }
        IVehicleRepository Vehicles { get; set; }
        ICommentRepository Comments { get; set; }

        int Complete();
    }
}
