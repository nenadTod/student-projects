using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentApp.Models.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Producer { get; set; }
        public int YearOfProduction { get; set; }
        public double PricePerHour { get; set; }
        public bool IsAvailable { get; set; }
        public string VehicleImage { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

       
    }
}