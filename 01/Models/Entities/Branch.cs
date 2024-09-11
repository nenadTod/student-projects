using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public bool Deleted { get; set; }

        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual AppUser Creator { get; set; }

        [Required]
        public int ServiceId { get; set; }     
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}