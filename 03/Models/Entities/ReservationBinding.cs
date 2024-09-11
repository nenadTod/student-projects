using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class ReservationBinding
    {
        public int ClientID { get; set; }
        public int GetBranchId { get; set; }
        public string GetDate { get; set; }
        public string GetTime { get; set; }
        public int ReturnBranchId { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnTime { get; set; }
        public int VehicleID { get; set; }       
       
       
    }
}