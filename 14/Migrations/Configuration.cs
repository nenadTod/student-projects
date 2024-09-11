namespace RentApp.Migrations
{
    using Models.Entities;
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

            Service ser = new Service
            {
                Name = "N1",
                LogoUrl = "",
                Email = "E1",
                Description = "D1",
                Aproved = true
            };
            
                context.Services.AddOrUpdate(ser);
            
            BranchOffice bo1 = new BranchOffice
            {
                ImageUrl = "",
                Adress = "A1",
                Longitude = 22,
                Latitude = 33
            };
           
                context.BrancheOffices.AddOrUpdate(bo1);
            
            Vehicle v1 = new Vehicle
            {
                PricePerHour = 333,
                Model = "M1",
                Producer = "P1",
                YearOfProduction = 2000,
                Description = "D1",
                ImageUrl = ""
            };
            
                context.Vehicles.AddOrUpdate(v1);
            
            BranchOffice bo2 = new BranchOffice
            {
                ImageUrl = "",
                Adress = "A2",
                Longitude = 12,
                Latitude = 23
            };
            
                context.BrancheOffices.AddOrUpdate(bo2);
            
            Vehicle v2 = new Vehicle
            {
                PricePerHour = 332,
                Model = "M2",
                Producer = "P2",
                YearOfProduction = 2001,
                Description = "D2",
                ImageUrl = ""
            };
            
                context.Vehicles.AddOrUpdate(v2);
            
            ser.BranchOffices = new System.Collections.Generic.List<BranchOffice>();
            ser.BranchOffices.Add(bo1);
            ser.BranchOffices.Add(bo2);

            bo1.Vehicles = new System.Collections.Generic.List<Vehicle>();
            bo1.Vehicles.Add(v1);

            bo2.Vehicles = new System.Collections.Generic.List<Vehicle>();
            bo2.Vehicles.Add(v2);

            User user1 = new User();
            user1.Approved = true;
            user1.Email = "asd";
            user1.Password = "asdasd";
            user1.Role = "Client";

            
                context._Users.Add(user1);
            
            User user2 = new User();
            user2.Approved = true;
            user2.Email = "qwe";
            user2.Password = "qweqwe";
            user2.Role = "Manager";

            
                context._Users.Add(user2);
            

            User user3 = new User();
            user3.Approved = true;
            user3.Email = "zxc";
            user3.Password = "zxczxc";
            user3.Role = "Admin";

            
                context._Users.Add(user3);
            



            context.SaveChanges();
            
           
        }
    }
}
