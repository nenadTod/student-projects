using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class VehicleFilterModel
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int YearOfProduction { get; set; }
        public int TypeId { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }
    }
}