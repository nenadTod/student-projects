using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Ratings", Schema = "dbo")]
    public class Rating
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int AppUserId { get; set; }

        [Column("CommentDate", TypeName = "datetime2")]
        public DateTime CommentDate { get; set; }
        public double Grade { get; set; }
    }
}