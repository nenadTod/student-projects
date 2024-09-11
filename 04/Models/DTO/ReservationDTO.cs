using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.DTO
{
    [NotMapped]
    public class ReservationDTO
    {

        [NotMapped]
        public double PriceToPay { get; set; }

        public int Id { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ReservedVehicleId { get; set; }
        public int BranchTakeId { get; set; }
        public int BranchDropOffId { get; set; }

        public ReservationDTO() { }

        public ReservationDTO(Reservation vehicle)
        {
            this.Id = vehicle.Id;
            this.BeginTime = vehicle.BeginTime;
            this.EndTime = vehicle.EndTime;
            this.ReservedVehicleId = vehicle.ReservedVehicleId;
            this.BranchDropOffId = vehicle.BranchDropOffId;
            this.BranchTakeId = vehicle.BranchTakeId;
            
        }

    }
}