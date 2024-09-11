using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Rent Rent { get; set; }
        public double Amount { get; set; }
    }
}