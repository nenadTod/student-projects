using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class VehicleImage
    {
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; }

        [ForeignKey("VehicleImageVehicle")]
        public int VehicleImageVehicleId { get; set; }
        public Vehicle VehicleImageVehicle { get; set; }


    }
}