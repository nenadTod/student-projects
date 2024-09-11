using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Persistance.Repository;
using System.Data.Entity;
using Unity.Attributes;

namespace RepoDemo.Persistance.UnitOfWork
{
    public class DemoUnitOfWork : IUnitOfWork 
    {
        private readonly DbContext _context;
      
        [Dependency]
        public IServiceRepository Services { get; set; }

        [Dependency]
        public IAppUserRepository AppUsers { get; set; }

        [Dependency]
        public IBranchRepository Branches { get; set; }

        [Dependency]
        public IRentRepository Rents { get; set; }

        [Dependency]
        public ITypeOfVehicleRepository TypeOfVehicles { get; set; }

        [Dependency]
        public IVehicleRepository Vehicles { get; set; }

        [Dependency]
        public ICommentRepository Comments { get; set; }

        [Dependency]
        public IGradeRepository Grades { get; set; }

        [Dependency]
        public INotificationRepository Notifications { get; set; }


        public DemoUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}