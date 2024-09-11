using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public bool Approved { get; set; }
        public string Name { get; set; }
        public List<BranchOffice> Offices { get; set; }
        public string ImagePath { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public int ManagerId { get; set; }
        public AppUser Manager { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public float Rate { get; set; }
        public int NumOfRates { get; set; }
        public List<Comment> Comments { get; set; }
        public Service()
        {

        }

        //public Service(string name, string image, AppUser manager, string description, string email)
        //{
        //    Name = name;
        //    ImagePath = image;
        //    Manager = manager;
        //    Description = description;
        //    Email = email;
        //    Approved = false;
        //    Offices = new List<BranchOffice>();
        //    Vehicles = new List<Vehicle>();
        //}
    }

}