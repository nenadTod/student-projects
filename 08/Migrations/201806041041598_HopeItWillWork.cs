namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HopeItWillWork : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(),
                        Branch_Id = c.Int(),
                        Vehicle_id = c.Int(),
                        AppUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.Branch_Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .Index(t => t.Branch_Id)
                .Index(t => t.Vehicle_id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Logo = c.String(),
                        Address = c.String(),
                        Latitude = c.Long(nullable: false),
                        Longitude = c.Long(nullable: false),
                        Vehicle_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_id)
                .Index(t => t.Vehicle_id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Model = c.String(),
                        Manufactor = c.String(),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        PricePerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unavailable = c.Boolean(nullable: false),
                        Type_Id = c.Int(),
                        Vehicle_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TypeOfVehicles", t => t.Type_Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_id)
                .Index(t => t.Type_Id)
                .Index(t => t.Vehicle_id);
            
            CreateTable(
                "dbo.TypeOfVehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Rents", "Vehicle_id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "Vehicle_id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "Type_Id", "dbo.TypeOfVehicles");
            DropForeignKey("dbo.Branches", "Vehicle_id", "dbo.Vehicles");
            DropForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches");
            DropIndex("dbo.Vehicles", new[] { "Vehicle_id" });
            DropIndex("dbo.Vehicles", new[] { "Type_Id" });
            DropIndex("dbo.Branches", new[] { "Vehicle_id" });
            DropIndex("dbo.Rents", new[] { "AppUser_Id" });
            DropIndex("dbo.Rents", new[] { "Vehicle_id" });
            DropIndex("dbo.Rents", new[] { "Branch_Id" });
            DropTable("dbo.TypeOfVehicles");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Branches");
            DropTable("dbo.Rents");
        }
    }
}
