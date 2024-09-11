using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Items", Schema = "dbo")]
    public class Item
    {
        public int Id { get; set; }
        public int PricelistId { get; set; }
        public int VehicleId { get; set; }
        public double Price { get; set; }
    }
}