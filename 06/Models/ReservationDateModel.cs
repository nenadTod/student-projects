using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class ReservationDateModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public ReservationDateModel() { }
    }
}