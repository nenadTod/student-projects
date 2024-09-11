using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Comments", Schema = "dbo")]
    public class Comment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int AppUserId { get; set; }
        public string FullNameUser { get; set; }

        [Column("CommentDate", TypeName = "datetime2")]
        public DateTime CommentDate { get; set; }
        public string Description { get; set; }

    }
}