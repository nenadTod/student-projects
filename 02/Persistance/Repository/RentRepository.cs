using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class RentRepository : Repository<Rent, int>, IRentRepository
    {   
        private RADBContext Context { get; set; }

        public RentRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<Rent> GetAll(int idVehicle)
        {
            return Context.Rents.Where(x=>x.Vehicle.Id==idVehicle).ToList();
        }

        public IEnumerable<Rent> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public bool IsFirstRentEnded(string email)
        {
            AppUser user = Context.AppUsers.First(x => x.Email == email);
            List<Rent> rents = user.Rents.OrderBy(x => x.Start).ToList();

            if (rents[0].End < DateTime.Now.Date)
                return true;

            return false;
        }
    }
}