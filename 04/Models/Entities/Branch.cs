using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

        [ForeignKey("BranchService")]
        public int BranchServiceId { get; set; }
        public Service BranchService { get; set; }

        [InverseProperty("BranchTake")]
        public List<Reservation> Reservations { get; set; }

        [InverseProperty("BranchDropOff")]
        public List<Reservation> ReservationsDropOff { get; set; } 
    }
}