using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Grade { get; set; }
        public virtual Service Service { get; set; }
        public virtual AppUser User { get; set; }
    }
}