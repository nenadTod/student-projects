using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public DateTime? GetVehicleDate { get; set; }
        public DateTime? ReturnVehicleDate { get; set; }
        [ForeignKey("BranchOffice")]

        public int GetBranchId { get; set; }
        public int ReturnBranchId { get; set; }
       
        public AppUser Client { get; set; }
        public Vehicle Vehicle { get; set; }
        public BranchOffice BranchOffice { get; set; }
    }
}