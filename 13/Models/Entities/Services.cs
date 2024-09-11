using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public bool Approved { get; set; }
        public string CreatorUserName { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public float AverageGrade { get; set; }
        public int NumberOfGrades { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; }
        public virtual List<Branch> Branches { get; set; }
        public virtual List<Comment> Comments{ get; set; }
    }
}