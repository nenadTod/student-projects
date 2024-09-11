using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string OrderID { get; set; }
        public string PaymentID { get; set; }
        public string PayerID { get; set; }
    }
}