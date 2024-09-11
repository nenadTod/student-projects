using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class VehiclePaginationResponse
    {
        public List<Vehicle> Vehicles { get; set; }
        public int Pages { get; set; }
    }
}