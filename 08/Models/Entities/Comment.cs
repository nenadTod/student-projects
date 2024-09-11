using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsNegative { get; set; }
        public DateTime? PostedDate { get; set; }
        public int UserKey { get; set; }
    }
}