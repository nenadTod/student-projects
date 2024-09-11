using RentApp.Persistance.Repository.Interfaces;
using RentApp.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace RentApp.Persistance.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
       private readonly DbContext _context;
     

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }
        //[Dependency]
        public IAppUserRepository AppUsers { get; set; }
        //[Dependency]
        public IBranchOfficeRepository BranchOffices { get; set; }
        //[Dependency]
        public IReservationRepository Reservations { get; set; }
        //[Dependency]
        public IVehicleRepository Vehicles { get; set; }
        //[Dependency]
        public IServiceRepository Services { get; set; }

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