using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Rent
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; } 
        public DateTime? End { get; set; }
        public bool Used { get; set; } 
        public virtual string BeginBranch { get; set; }
        public virtual string EndBranch { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual AppUser User { get; set; }
    }
}