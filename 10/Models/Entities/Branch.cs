using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Branch
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }
    }
}