using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [ForeignKey("RAIdentityUser")]
        public string RAIdentityUserId { get; set; }
        public RAIdentityUser RAIdentityUser { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public string Content { get; set; }

        public Comment() { }
    }
}