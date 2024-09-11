using RentApp.Models;
using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RentApp.Persistance.Repository
{
    public class VehicleRepository : Repository<Vehicle, int>, IVehicleRepository
    {
        public VehicleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize)
        {
            List<Vehicle> vehicles = new List<Vehicle>(RADBContext.Vehicles.OrderBy(v => v.Manufactor).Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return vehicles;
        }

        public IEnumerable<Vehicle> Search(string search)
        {
            int price = 0;
            int.TryParse(search, out price);
            search = search.ToLower();

            return RADBContext.Vehicles.Where(v => v.Model.ToLower().Contains(search) || v.Manufactor.ToLower().Contains(search) || v.Type.Name.ToLower().Contains(search) || v.Description.ToLower().Contains(search) || v.PricePerHour == price);
        }

        public IEnumerable<Vehicle> Filter(Search search)
        {

            List<Vehicle> vehicles = new List<Vehicle>();
            List<Vehicle> all = new List<Vehicle>(RADBContext.Vehicles);

            if (search.ServiceId > 0)
            {
                List<Branch> branches = new List<Branch>(RADBContext.Branches.Where(b => b.ServiceId == search.ServiceId));

                for (int i = 0; i < branches.Count; i++)
                {
                    vehicles.AddRange(all.Where(v => v.BranchId == branches[i].Id).ToList());
                }
            }

            if (search.BranchId > 0)
            {
                vehicles = all.Where(v => v.BranchId == search.BranchId).ToList();
            }

            if (search.TypeId > 0)
            {
                vehicles = all.Where(v => v.TypeId == search.TypeId).ToList();
            }

            if (search.Model != "")
            {
                vehicles = all.Where(v => v.Model.ToLower().Equals(search.Model.ToLower())).ToList();
            }

            if (search.Manufactor != "")
            {
                vehicles = all.Where(v => v.Manufactor.ToLower().Equals(search.Manufactor.ToLower())).ToList();
            }

            if (search.Description != "")
            {
                vehicles = all.Where(v => v.Description.ToLower().Equals(search.Description.ToLower())).ToList();
            }

            if(search.Price1 > search.Price2 || search.Price1 < 0 || search.Price2 < 0)
            {
                vehicles.Clear();
            }
            else if (search.Price1 > 0 && search.Price2 > 0)
            {
                vehicles = all.Where(v => v.PricePerHour <= search.Price2 && v.PricePerHour >= search.Price1).ToList();
            }

            if (search.Year != null)
            {
                vehicles = all.Where(v => v.Year.Equals(search.Year)).ToList();
            }

            return vehicles;
        }

        protected RADBContext RADBContext { get { return context as RADBContext; } }
    }
}