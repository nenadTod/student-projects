using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity.Attributes;

namespace RentApp.Persistance.UnitOfWork
{
    public class RAUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        [Dependency]
        public IServiceRepository Services { get; set; }

        [Dependency]
        public IAppUserRepositroy AppUsers { get; set; }

        [Dependency]
        public IVehicleRepository Vehicles { get; set; }

        [Dependency]
        public IBranchOfficeRepository BranchOffice { get; set; }

        [Dependency]
        public IReservationRepository Reservations { get; set; }

        [Dependency]
        public ICommentRepository Comments { get; set; }

        public RAUnitOfWork(DbContext context)
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