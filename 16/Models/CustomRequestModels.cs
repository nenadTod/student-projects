using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{

    public class BranchOfficeRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Logo { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int ServiceId { get; set; }
    }

    public class ReviewRequestModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Comment { get; set; }
        public int Stars { get; set; }
        public int ServiceId { get; set; }
    }

    public class VehicleRequestModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        //public string Logo { get; set; }
        public string Manufactor { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal PricePerHour { get; set; }
        public int ServiceId { get; set; }
        public bool Unavailable { get; set; }
    }

    public class PromotedUserRequestModel
    {
        public int UserId { get; set; }
        public string NewRole { get; set; }
    }

    public class SearchVehicleRequestModel
    {
        public string Model { get; set; }
        public string Manufactor { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public int PricePerHour { get; set; }
        public int Page { get; set; }
    }

    public class RentRequestModel
    {
        public int BranchOfficeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int VehicleId { get; set; }
    }
}