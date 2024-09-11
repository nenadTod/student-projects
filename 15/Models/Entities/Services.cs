using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Services", Schema = "dbo")]
    public partial class Service
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }

    }
}