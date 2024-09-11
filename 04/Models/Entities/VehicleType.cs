using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
                
    }
}