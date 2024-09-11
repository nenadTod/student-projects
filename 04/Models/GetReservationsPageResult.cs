using RentApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class GetReservationsPageResult
    {
        public IEnumerable<ReservationDTO> Reservations { get; set; }
        public double PriceToPay { get; set; }
    }
}