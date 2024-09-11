using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Activated { get; set; }

        public bool Deleted { get; set; }


        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual AppUser Creator { get; set; }


        public virtual List<Vehicle> Vehicles { get; set; }
        public virtual List<Branch> Branches { get; set; }
    }
}