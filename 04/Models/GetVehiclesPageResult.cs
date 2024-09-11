using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class GetVehiclesPageResult
    {
        public IEnumerable<VehicleDTO> Vehicles { get; set; }
        public int Count { get; set; }
    }
}