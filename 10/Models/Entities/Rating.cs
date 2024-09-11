using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Rating
    {
        public int Id { get; set; }

        public virtual AppUser User { get; set; }

        public virtual Service Service { get; set; }

        public string TypeOfVote { get; set; }
    }
}