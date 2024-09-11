using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Column("Birthday", TypeName ="datetime2")]
        public DateTime Birthday { get; set; }
        public string PicturePath { get; set; }
        public bool IsUserConfirmed { get; set; }
        public bool IsManagerAllowed { get; set; }

        public List<Service> ManagedServices { get; set; } 
        public List<Reservation> Reservations { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}