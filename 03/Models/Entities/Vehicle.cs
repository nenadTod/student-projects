using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        public string Model { get; set; }
        public string Producer { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Available { get; set; }
      
        public VehicleType VehicleType { get; set; }
        public Service Service { get; set; }
       

    }
}