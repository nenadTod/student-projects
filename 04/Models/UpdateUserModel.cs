using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Column("Birthday", TypeName = "datetime2")]
        public DateTime Birthday { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}