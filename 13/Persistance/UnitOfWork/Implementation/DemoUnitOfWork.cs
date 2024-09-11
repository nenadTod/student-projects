using RentApp.Persistance.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentApp.Persistance.Repository.Interface;
using System.Data.Entity;
using Unity.Attributes;

namespace RentApp.Persistance.UnitOfWork.Implementation
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
        public ITypeOfVehicleRepository Types { get; set; }

        [Dependency]
        public IVehicleRepository Vehicles { get; set; }

        [Dependency]
        public ICommentRepository Comments { get; set; }

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