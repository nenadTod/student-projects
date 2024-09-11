using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool Authorized { get; set; }
        public int ManagerId { get; set; }

        public virtual List<Vehicle> Vehicles { get; set; }
        public virtual List<BranchOffice> BranchOffices { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}