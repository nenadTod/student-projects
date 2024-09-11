using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        [Column("BeginTime", TypeName = "datetime2")]
        public DateTime BeginTime { get; set; }

        [Required]
        [Column("EndTime", TypeName = "datetime2")]
        public DateTime EndTime { get; set; }


        public bool Payed { get; set; }
       
        public string PaymentId {get; set;}

        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser User { get; set; }

        [ForeignKey("ReservedVehicle")]
        public int ReservedVehicleId { get; set; }
        public Vehicle ReservedVehicle { get; set; }

        [Required]
        [ForeignKey("BranchTake")]
        public int BranchTakeId { get; set; }
        public Branch BranchTake { get; set; }

        [ForeignKey("BranchDropOff")]
        public int BranchDropOffId { get; set; }
        public Branch BranchDropOff { get; set; }
    }
}