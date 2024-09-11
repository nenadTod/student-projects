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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LogoImagePath { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email address.")]
        public string Email { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }

        [ForeignKey("ServiceManager")]
        public int ServiceManagerId { get; set; }
        public AppUser ServiceManager { get; set; }

        public List<Branch> Branches { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Pricelist> Pricelists { get; set; }

    }
}