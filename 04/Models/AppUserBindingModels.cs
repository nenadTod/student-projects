using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class VerifyAppUserBindingModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
    }
}