using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class Search
    {
        public int ServiceId { get; set; }
        public int BranchId { get; set; }
        public int TypeId { get; set; }
        public string Manufactor { get; set; }
        public string Model { get; set; } 
        public DateTime? Year { get; set; }
        public string Description { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }

    }
}