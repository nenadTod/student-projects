namespace RentApp.Migrations
{
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

                p => p.FullName,

                new AppUser() { FullName = "AppUser AppUserovic" }

            );

            context.AppUsers.AddOrUpdate(
                m=>m.FullName,
                new AppUser() { FullName = "Manager Managerovic"}
            );

            //var tov1 = new TypeOfVehicle() { Name = "Karavan" };
            //var tov2 = new TypeOfVehicle() { Name = "Limuzina" };

            //var v1 = new Vehicle() { Manufactor = "Zastava", Model = "101", PricePerHour = 10.4m, Type = tov1, Year = 1969 };
            //var v2 = new Vehicle() { Manufactor = "Zastava", Model = "Yugo Koral", PricePerHour = 10.4m, Type = tov2, Year = 1988 };

            //var b1 = new Branch() { Address = "Sime Solaje 24", Latitude = 123424.2, Longitude = 123232.4 };

            //var s1 = new Service() { Name = "Zastava rent", Email = "kontakt@zastava.rs", Description = "Nepobedivi", Branches = new List<Branch>() { b1 }, Vehicles = new List<Vehicle>() { v1, v2 } };

            //context.Services.Add(s1);

            //SaveChanges(context);


            var userStore = new UserStore<RAIdentityUser>(context);
            var userManager = new UserManager<RAIdentityUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "Admin Adminovic");
                var user = new RAIdentityUser() { Id = "admin", UserName = "admin", Email = "admin@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("admin"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu"))

            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "AppUser AppUserovic");
                var user = new RAIdentityUser() { Id = "appu", UserName = "appu", Email = "appu@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("appu"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }

            if (!context.Users.Any(u => u.UserName == "manager"))

            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.FullName == "Manager Managerovic");
                var user = new RAIdentityUser() { Id = "manager", UserName = "manager", Email = "manager@yahoo.com", PasswordHash = RAIdentityUser.HashPassword("manager"), AppUserId = _appUser.Id };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Manager");
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
