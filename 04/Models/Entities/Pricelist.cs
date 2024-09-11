using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Pricelist
    {
        public int Id { get; set; }
        [Required]
        [Column("BeginTime", TypeName = "datetime2")]
        public DateTime BeginTime { get; set; }
        [Required]
        [Column("EndTime", TypeName = "datetime2")]
        public DateTime EndTime { get; set; }
        
        public List<Item> Items { get; set; }

        [ForeignKey("PricelistService")]
        public int PricelistServiceId { get; set; }
        public Service PricelistService { get; set; }

    }
}