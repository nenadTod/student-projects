using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentApp.Persistance.Repository;
using System.Data.Entity;
using Unity.Attributes;

namespace RentApp.Persistance.UnitOfWork
{
    public class RADBUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        [Dependency]
        public IServiceRepository Services { get; set; }

        [Dependency]
        public IUserRepository Users { get; set; }

        [Dependency]
        public IBranchRepository Branches { get; set; }

        [Dependency]
        public IToVRepository TypesOfVehicle { get; set; }

        [Dependency]
        public IRentRepository Rents { get; set; }

        [Dependency]
        public IVehicleRepository Vehicles { get; set; }

        [Dependency]
        public ICommentRepository Comments { get; set; }

        public RADBUnitOfWork(DbContext context)
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