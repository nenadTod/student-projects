using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("AppUsers", Schema = "dbo")]
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        [Column("BirthDate", TypeName ="datetime2")]
        public DateTime? Birthday { get; set; }
        public string Picture { get; set; }
        public bool CanMakeReservation { get; set; }
        public bool CanAddSercvice { get; set; }
    }
}