﻿using RentApp.Models.Entities;
using System.Collections.Generic;

namespace RentApp.Persistance.Repository
{
    public interface IBranchRepository : IRepository<Branch, int>
    {
        IEnumerable<Branch> GetAll(int pageIndex, int pageSize);
    }
}