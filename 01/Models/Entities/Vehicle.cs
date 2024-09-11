using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }     
        
        [Required]  
        public string Model { get; set; }

        [Required]
        public string Manufactor { get; set; }

        [Required]
        public int? Year { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal PricePerHour { get; set; }

        [Required]
        public bool Unavailable { get; set; }

        [Required]
        public virtual string Images { get; set; }

        public bool Deleted { get; set; }

        [Required]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual  TypeOfVehicle Type { get; set; }


        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual AppUser Creator { get; set; }


        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}