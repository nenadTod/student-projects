using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using RentApp.Persistance.Repository;
using System;
using System.Data.Entity;

namespace RepoDemo.Persistance.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository Services { get; set; }
        IAppUserRepository AppUsers { get; set; }
        IBranchRepository Branches { get; set; }
        IRentRepository Rents { get; set; }
        ITypeOfVehicleRepository TypeOfVehicles { get; set; }
        IVehicleRepository Vehicles { get; set; }
        ICommentRepository Comments { get; set; }
        IGradeRepository Grades { get; set; }
        INotificationRepository Notifications { get; set; }


        int Complete();
    }
}
