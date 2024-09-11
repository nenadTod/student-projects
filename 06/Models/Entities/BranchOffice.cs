using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace RentApp.Models.Entities
{
    public class BranchOffice
    {
        public BranchOffice()
        {
        }

        public BranchOffice(string imagePath, string address, double longitude, double latitude, Service service) : this()
        {
            ImagePath = imagePath;
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
            Service = service;
        }

        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}