﻿using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Persistance.Repository
{
    public interface IGradeRepository : IRepository<Grade, int>
    {
    }
}