using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {
        public VehicleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Vehicle> GetAllWithImages()
        {
            return context.Set<Vehicle>().Include("Images").ToList();
        }

        public IEnumerable<Vehicle> GetAllAvailableWithImages()
        {
            return context.Set<Vehicle>().Where(v=> v.IsAvailable == true).Include("Images").ToList();
        }

        public IEnumerable<Vehicle> GetAllOfService(int serviceId)
        {
            return context.Set<Vehicle>().Include("Images").Where(v => v.VehicleServiceId == serviceId);
        }

        public int Count()
        {
            return context.Set<Vehicle>().Count();
        }
        
        public IEnumerable<Vehicle> GetVehiclePageWithImages(int pageIndex, int pageSize)
        {
            return context.Set<Vehicle>().Include("Images").OrderBy(v => v.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Vehicle> GetAvailableVehiclePageWithImages(int pageIndex, int pageSize)
        {
            return context.Set<Vehicle>().Include("Images").Where(ve => ve.IsAvailable == true).OrderBy(v => v.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Vehicle> GetVehiclePage(int pageIndex, int pageSize)
        {
            return context.Set<Vehicle>().Include("Images").OrderBy(v => v.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}