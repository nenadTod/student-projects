using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace RentApp.Persistance
{
    public class RADBContext : IdentityDbContext<RAIdentityUser>
    {
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PriceList> Pricelists { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        
        public RADBContext() : base("name=RADB")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static RADBContext Create()
        {
            return new RADBContext();
        }
    }
}