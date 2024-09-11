using RentApp.Models.Entities;
using RentApp.Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public class BranchOfficeRepository:Repository<BranchOffice,int>,IBranchOfficeRepository
    {
        protected RADBContext rADBContext { get { return context as RADBContext; } }

        public BranchOfficeRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<BranchOffice> GetAll(int pageIndex, int pageSize)
        {
            return rADBContext.BrancheOffices.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}