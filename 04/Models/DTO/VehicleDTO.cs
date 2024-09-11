using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [NotMapped]
    public class VehicleDTO 
    {
        [NotMapped]
        public double PricePerHour { get; set; }

        public int Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int YearOfProduction { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public List<Item> Items { get; set; }
        public List<VehicleImage> Images { get; set; }
        public List<Reservation> Reservations { get; set; }
        public int TypeId { get; set; }
        public int VehicleServiceId { get; set; }

        public VehicleDTO() { }

        public VehicleDTO(Vehicle vehicle)
        {
            this.Id = vehicle.Id;
            this.Model = vehicle.Model;
            this.Manufacturer = vehicle.Manufacturer;
            this.YearOfProduction = vehicle.YearOfProduction;
            this.Description = vehicle.Description;
            this.IsAvailable = vehicle.IsAvailable;
            this.Items = vehicle.Items;
            this.Images = vehicle.Images;
            this.Reservations = vehicle.Reservations;
            this.TypeId = vehicle.TypeId;
            this.VehicleServiceId = vehicle.VehicleServiceId;
        }
    }
}