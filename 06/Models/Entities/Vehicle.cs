using System.ComponentModel.DataAnnotations.Schema;

namespace RentApp.Models.Entities
{
    public class Vehicle
    {
        public Vehicle()
        {
            Available = true;
        }

        //public Vehicle(string type, string model, string desctription, int pricePerHour, string filePath, Service service, string manifacturer, int productionYear) : this()
        //{
        //    Type = type;
        //    Model = model;
        //    Description = desctription;
        //    Price = pricePerHour;
        //    ImagePath = filePath;
        //    Service = service;
        //    Manifacturer = manifacturer;
        //    ProductionYear = productionYear;
        //}
        

        public int Id { get; set; }
        public bool Available { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ProductionYear { get; set; }
        public string Manifacturer { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service{ get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
        
    }
}