using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Persistance
{
    public class RADBContext : IdentityDbContext<RAIdentityUser>
    {
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<BranchOffice> Offices { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }



        public Guid id;
        public RADBContext() : base("name=RADB")
        {
            id = Guid.NewGuid();
        }

        public static RADBContext Create()
        {
            return new RADBContext();
        }
    }
}