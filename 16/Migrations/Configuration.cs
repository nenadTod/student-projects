using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
 
namespace RentApp.Migrations
{


    internal sealed class Configuration : DbMigrationsConfiguration<RentApp.Persistance.RADBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RentApp.Persistance.RADBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Manager" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            context.AppUsers.AddOrUpdate(

                  u => u.FullName,

                  new AppUser() { FullName = "Admin Adminovic" }

            );

            context.AppUsers.AddOrUpdate(

                  u => u.FullName,

                  new AppUser() { FullName = "Menadzer Menadzerovic", Email="menadzer@menadzerovic.com", Birthday = DateTime.Now, Activated = true }

            );

            context.AppUsers.AddOrUpdate(

                p => p.FullName,

                new AppUser() { FullName = "AppUser AppUserovic" , Email= "sofija@gmail.com", Id=12345 }

            );

            context.AppUsers.AddOrUpdate(

                s => s.FullName,

                new AppUser() { FullName = "Sofija Ilic", Email = "sofija4@gmail.com", Id = 1234567890 }

            );


            //BranchOffice bo = new BranchOffice() { Address = "Random adresa", Latitude = 45.267136, Longitude = 19.833549, Logo = "logo.png", Name = "Office1" };

            //Vehicle vozilo = new Vehicle();

            //VehicleType vt = new VehicleType() { Name = "Limuzina" };

            //vozilo.Model = "Audi";
            //vozilo.Manufactor = "Proizvodjac1";
            //vozilo.Year = 2015;
            //vozilo.Description = "asdasdas";
            //vozilo.Type = vt;
            //vozilo.PricePerHour = 10;
            //vozilo.Unavailable = false;
            //vozilo.Images = new List<string>() { "slika1.png" };

            //vt.Vehicles = new List<Vehicle>() { vozilo };

            //Service s1 = new Service() { BranchOffices = new List<BranchOffice>() { bo }, Email = "test@mail.com", Description = "asdasdasd", Name = "test name", Logo = "logo.png", Vehicles = new List<Vehicle>() { vozilo } };

            //Rent r1 = new Rent() { BranchOffice = bo, Start = DateTime.Now, End = DateTime.Now, Vehicle = vozilo };

            //AppUser appUser = new AppUser() { FullName = "Pera Petrovic", Email = "perapetrovic@gmail.com", Birthday = DateTime.Now, Image = "slika.jpg", Activated = false, Rents = new List<Rent>() { r1 } };

            //VehicleType vt = new VehicleType() { Name = "Limuzina" };
            //Vehicle v1 = new Vehicle() { Model = "Punto", Description = "asdasdasd", Images = new List<string>() { "slika.jpg" }, Manufactor = "Fiat", PricePerHour = 10, Type = vt, Unavailable = false, Year = 2015 };
            //vt.Vehicles = new List<Vehicle>() { v1 };

            //BranchOffice bo = new BranchOffice() { Address = "Adresa1", Latitude = 45.123123, Longitude = 19.123123, Logo = "logo.jpg", Name = "Office1" };

            //Rent r1 = new Rent() { BranchOffice = bo, Start = DateTime.Now, End = DateTime.Now, Vehicle = v1 };

            //AppUser appuser = new AppUser() { Activated = false, Birthday = DateTime.Now, FullName = "User Userovic", Email = "user@userovic.com", Image = "slika.jpg", Rents = new List<Rent>() { r1 } };

            //Service s1 = new Service() { BranchOffices = new List<BranchOffice>() { bo }, Description = "adasdasd", Email = "service1@service.com", Logo = "logo1.png", Name = "Service1", Vehicles = new List<Vehicle>() { v1 } };


            //context.AppUsers.AddOrUpdate(

            //     e => e.Email,

            //    appuser

            //);

            //context.BranchOffices.AddOrUpdate(

            //     b => b.Name,

            //    bo

            //);

            //context.VehicleTypes.AddOrUpdate(

            //     v => v.Name,

            //    vt

            //);

            //context.Vehicles.AddOrUpdate(

            //     v => v.Model,

            //    v1

            //);

            //context.Services.AddOrUpdate(

            //     s => s.Name,

            //    s1

            //);

            //context.Rents.AddOrUpdate(

            //     r => r.Id,

            //    r1

            //);


            SaveChanges(context);

            var userStore = new UserStore<RAIdentityUser>(context);
            var userManager = new UserManager<RAIdentityUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "Admin Adminovic");
                var user = new RAIdentityUser() { Id = "admin", UserName = "admin", Email = "admin@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("admin"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "menadzerko"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "Menadzer Menadzerovic");
                var user = new RAIdentityUser() { Id = "menadzer@menadzerovic.com", UserName = "menadzer@menadzerovic.com", Email = "menadzer@menadzerovic.com", PasswordHash = RAIdentityUser.HashPassword("menadzer"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Manager");
            }


            if (!context.Users.Any(u => u.UserName == "appu"))

            {

                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "AppUser AppUserovic");
                var user = new RAIdentityUser() { Id = "appu", UserName = "appu", Email = "appu@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("appu"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");

            }
        }
        private static void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
        }
    }

}
