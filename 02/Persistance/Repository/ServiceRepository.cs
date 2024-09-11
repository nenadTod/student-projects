using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        protected RADBContext Context { get { return context as RADBContext; } }
        public ServiceRepository(DbContext context) : base(context)
        {

        }

        protected RADBContext DemoContext { get { return context as RADBContext; } }

        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vehicle> GetVehicles(int serviceId)
        {
            return Context.Vehicles.Where(x => x.Service.Id == serviceId).ToList();
        }

        public IEnumerable<Branch> GetBranches(int serviceId)
        {
            return Context.Branches.Where(x => x.Service.Id == serviceId).ToList();
        }

        public void DeleteVehicles(int serviceId)
        {
            List<Vehicle> vehicles = Context.Vehicles.Where(x => x.Service.Id == serviceId).ToList();
            foreach(var v in vehicles)
            {
                Context.Vehicles.Remove(v);
            }
        }

        public void DeleteBranches(int serviceId)
        {
            List<Branch> branches = Context.Branches.Where(x => x.Service.Id == serviceId).ToList();
            foreach(var b in branches)
            {
                Context.Branches.Remove(b);
            }
        }

        public IEnumerable<Comment> GetComments(int serviceId)
        {
            return Context.Comments.Where(x => x.Service.Id == serviceId).ToList();
        }

        public void DeleteComments(int serviceId)
        {
            List<Comment> comments = Context.Comments.Where(x => x.Service.Id == serviceId).ToList();
            foreach (var c in comments)
            {
                Context.Comments.Remove(c);
            }
        }

        public IEnumerable<Service> GetActiveServices()
        {
            return Context.Services.Where(x => x.Activated == true).ToList();
        }

        public IEnumerable<Service> GetDeactiveServices()
        {
            return Context.Services.Where(x => x.Activated == false).ToList();
        }
    }
}