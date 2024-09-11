using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public string PersonalDocument { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsProcessed { get; set; }
        public virtual List<Rent> Rents { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}