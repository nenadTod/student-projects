namespace RentApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RentApp.Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

                  new AppUser() { FullName = "Admin Adminovic" , Email = "admin@yahoo.com" }

            );

            context.AppUsers.AddOrUpdate(

                p => p.FullName,

                new AppUser() { FullName = "AppUser AppUserovic", Email = "appu@yahoo.com" }

            );

            //Service ser1 = new Service() { Name = "RentACar", Email = "rent@gmail.com" };
            TypeOfVehicle type1 = new TypeOfVehicle() { Name = "sport" };
            TypeOfVehicle type2 = new TypeOfVehicle() { Name = "jeep" };
            Vehicle vehicle1 = new Vehicle() { Model = "A3", Manufactor = "Audi", Year = 2011, PricePerHour = 20, Type = type1 };
            Vehicle vehicle2 = new Vehicle() { Model = "Range Rover", Manufactor = "Toyota", Year = 2014, PricePerHour = 30, Type = type2 };
            Vehicle vehicle3 = new Vehicle() { Model = "C3", Manufactor = "Renault", Year = 2013, PricePerHour = 10, Type = type1 };
            Branch branch1 = new Branch() { Latitude = 46.52, Longitude = 82.65 };
            Branch branch2 = new Branch() { Latitude = 32.12, Longitude = 95.16 };
            //ser1.Branches = new System.Collections.Generic.List<Branch>();
            //ser1.Branches.Add(branch1);
            //ser1.Branches.Add(branch2);
            //ser1.Vehicles = new System.Collections.Generic.List<Vehicle>();
            //ser1.Vehicles.Add(vehicle1);
            //ser1.Vehicles.Add(vehicle2);
            //ser1.Vehicles.Add(vehicle3);

            //Service ser2 = new Service() { Name = "MiKat", Email = "mikat@gmail.com" };
            Branch branch21 = new Branch() { Latitude = 26.45, Longitude = 65.96 };
            Branch branch22 = new Branch() { Latitude = 36.75, Longitude = 68.32 };
            //ser2.Branches = new System.Collections.Generic.List<Branch>();
            //ser2.Branches.Add(branch21);
            //ser2.Branches.Add(branch22);
            //ser2.Vehicles = new System.Collections.Generic.List<Vehicle>();
            //ser2.Vehicles.Add(vehicle2);


            context.Services.AddOrUpdate(
                s => s.Name,
                new Service() { Name = "RentACar", Email = "rent@gmail.com" , Branches = new System.Collections.Generic.List<Branch>() { branch1, branch2}, Vehicles = new System.Collections.Generic.List<Vehicle>() { vehicle1,vehicle2,vehicle3} }
                );

            context.Services.AddOrUpdate(
                m => m.Name,
                new Service() { Name = "MiKat", Email = "mikat@gmail.com" , Branches = new System.Collections.Generic.List<Branch>() { branch21,branch22}, Vehicles = new System.Collections.Generic.List<Vehicle>() { vehicle2} }
                );

            context.SaveChanges();

            var userStore = new UserStore<RAIdentityUser>(context);
            var userManager = new UserManager<RAIdentityUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "Admin Adminovic");
                var user = new RAIdentityUser() { Id = "admin@yahoo.com", UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("admin"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu"))

            {

                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "AppUser AppUserovic");
                var user = new RAIdentityUser() { Id = "appu@yahoo.com", UserName = "appu@yahoo.com", Email = "appu@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("appu"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");

            }
        }

        // metoda za bolji prikaz greske
        //private static void SaveChanges(DbContext context)
        //{
        //    try
        //    {
        //        context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var sb = new StringBuilder();
        //        foreach (var failure in ex.EntityValidationErrors)
        //        {
        //            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
        //            foreach (var error in failure.ValidationErrors)
        //            {
        //                sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
        //                sb.AppendLine();
        //            }
        //        }
        //        throw new DbEntityValidationException(
        //            "Entity Validation Failed - errors follow:\n" +
        //            sb.ToString(), ex
        //        );
        //    }
        //}
    }
}
