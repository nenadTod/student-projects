using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }

        [ForeignKey("ItemPriceList")]
        public int ItemPriceListId { get; set; }
        public Pricelist ItemPriceList { get; set; }

        [ForeignKey("ItemVehicle")]
        public int ItemVehicleId { get; set; }
        public Vehicle ItemVehicle { get; set; }
    }
}