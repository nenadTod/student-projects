using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateTime { get; set; }
        public string Text { get; set; }
        public virtual AppUser Author { get; set; }
    }
}