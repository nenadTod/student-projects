using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Approved { get; set; }
        public string ImagePath { get; set; }
        public bool Blocked { get; set; }

        public AppUser()
        {
            Approved = false;
            Blocked = false;
        }
    }
}