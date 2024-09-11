using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;

namespace RentApp.Persistance.Repository.Implementations
{
    public class BranchOfficeRepository : Repository<BranchOffice, int>, IBranchOfficeRepository
    {
        public BranchOfficeRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<BranchOffice> GetRange(int pageIndex, int pageSize)
        {
            return Context.Offices.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<BranchOffice> GetOfficesFromServiceId(int serviceId)
        {
            return Context.Offices.Where(o => o.ServiceId == serviceId);
        }

        public void UploadImage(string image, int id)
        {
            var office = Context.Offices.FirstOrDefault(u => u.Id == id);
            office.ImagePath = image;
            Context.Entry(office).State = EntityState.Modified;
            Context.SaveChanges();
        }

        protected RADBContext Context => context as RADBContext;
    }
}