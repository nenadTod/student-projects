namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seen = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotificationTypes", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.Int(nullable: false),
                        ReservedVehicleId = c.Int(nullable: false),
                        BranchTakeId = c.Int(nullable: false),
                        BranchDropOffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchTakeId, cascadeDelete: true)
                .ForeignKey("dbo.Branches", t => t.BranchDropOffId, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.ReservedVehicleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ReservedVehicleId)
                .Index(t => t.BranchTakeId)
                .Index(t => t.BranchDropOffId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Address = c.String(),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        BranchServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.BranchServiceId, cascadeDelete: true)
                .Index(t => t.BranchServiceId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CommentedServiceId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.CommentedServiceId, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.CommentedServiceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PricelistServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.PricelistServiceId, cascadeDelete: true)
                .Index(t => t.PricelistServiceId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        ItemPriceListId = c.Int(nullable: false),
                        ItemVehicleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pricelists", t => t.ItemPriceListId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.ItemVehicleId, cascadeDelete: true)
                .Index(t => t.ItemPriceListId)
                .Index(t => t.ItemVehicleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Model = c.String(),
                        Manufacturer = c.String(),
                        YearOfProduction = c.Int(nullable: false),
                        Description = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        VehicleServiceId = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleTypes", t => t.Type_Id)
                .ForeignKey("dbo.Services", t => t.VehicleServiceId, cascadeDelete: false)
                .Index(t => t.VehicleServiceId)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.VehicleImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        VehicleImageVehicleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleImageVehicleId, cascadeDelete: true)
                .Index(t => t.VehicleImageVehicleId);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AppUsers", "LastName", c => c.String());
            AddColumn("dbo.AppUsers", "Birthday", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AppUsers", "PicturePath", c => c.String());
            AddColumn("dbo.AppUsers", "IsUserConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "IsManagerAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "LogoImagePath", c => c.String());
            AddColumn("dbo.Services", "Email", c => c.String());
            AddColumn("dbo.Services", "Description", c => c.String());
            AddColumn("dbo.Services", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "ServiceManagerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "ServiceManagerId");
            AddForeignKey("dbo.Services", "ServiceManagerId", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.Reservations", "ReservedVehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Reservations", "BranchDropOffId", "dbo.Branches");
            DropForeignKey("dbo.Reservations", "BranchTakeId", "dbo.Branches");
            DropForeignKey("dbo.Branches", "BranchServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "ServiceManagerId", "dbo.AppUsers");
            DropForeignKey("dbo.Pricelists", "PricelistServiceId", "dbo.Services");
            DropForeignKey("dbo.Items", "ItemVehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "VehicleServiceId", "dbo.Services");
            DropForeignKey("dbo.Vehicles", "Type_Id", "dbo.VehicleTypes");
            DropForeignKey("dbo.VehicleImages", "VehicleImageVehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Items", "ItemPriceListId", "dbo.Pricelists");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.Comments", "CommentedServiceId", "dbo.Services");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.Notifications", "TypeId", "dbo.NotificationTypes");
            DropIndex("dbo.VehicleImages", new[] { "VehicleImageVehicleId" });
            DropIndex("dbo.Vehicles", new[] { "Type_Id" });
            DropIndex("dbo.Vehicles", new[] { "VehicleServiceId" });
            DropIndex("dbo.Items", new[] { "ItemVehicleId" });
            DropIndex("dbo.Items", new[] { "ItemPriceListId" });
            DropIndex("dbo.Pricelists", new[] { "PricelistServiceId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "CommentedServiceId" });
            DropIndex("dbo.Services", new[] { "ServiceManagerId" });
            DropIndex("dbo.Branches", new[] { "BranchServiceId" });
            DropIndex("dbo.Reservations", new[] { "BranchDropOffId" });
            DropIndex("dbo.Reservations", new[] { "BranchTakeId" });
            DropIndex("dbo.Reservations", new[] { "ReservedVehicleId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "TypeId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropColumn("dbo.Services", "ServiceManagerId");
            DropColumn("dbo.Services", "IsConfirmed");
            DropColumn("dbo.Services", "Description");
            DropColumn("dbo.Services", "Email");
            DropColumn("dbo.Services", "LogoImagePath");
            DropColumn("dbo.AppUsers", "IsManagerAllowed");
            DropColumn("dbo.AppUsers", "IsUserConfirmed");
            DropColumn("dbo.AppUsers", "PicturePath");
            DropColumn("dbo.AppUsers", "Birthday");
            DropColumn("dbo.AppUsers", "LastName");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.VehicleImages");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Items");
            DropTable("dbo.Pricelists");
            DropTable("dbo.Comments");
            DropTable("dbo.Branches");
            DropTable("dbo.Reservations");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.Notifications");
        }
    }
}
