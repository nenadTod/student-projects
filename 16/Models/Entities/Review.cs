using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string User { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Comment { get; set; }
        public int Stars { get; set; }
    }
}