namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchOffices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        Picture = c.String(),
                        Addres = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PricelistId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ToDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Grade = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        FirstBranchOfficeId = c.Int(nullable: false),
                        SecundBranchOfficeId = c.Int(nullable: false),
                        DateRezervation = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ReturnDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchOffices", t => t.FirstBranchOfficeId, cascadeDelete: false)
                .ForeignKey("dbo.BranchOffices", t => t.SecundBranchOfficeId, cascadeDelete: false)
                .Index(t => t.FirstBranchOfficeId)
                .Index(t => t.SecundBranchOfficeId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        TypeVehicle = c.String(),
                        Produce = c.String(),
                        Model = c.String(),
                        ProductionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Price = c.Double(nullable: false),
                        Description = c.String(),
                        Available = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AppUsers", "BirthDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.AppUsers", "Picture", c => c.String());
            AddColumn("dbo.AppUsers", "CenMakeRezervation", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "CanAddSercvice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "AppUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Services", "Logo", c => c.String());
            AddColumn("dbo.Services", "Email", c => c.String());
            AddColumn("dbo.Services", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "SecundBranchOfficeId", "dbo.BranchOffices");
            DropForeignKey("dbo.Reservations", "FirstBranchOfficeId", "dbo.BranchOffices");
            DropIndex("dbo.Reservations", new[] { "SecundBranchOfficeId" });
            DropIndex("dbo.Reservations", new[] { "FirstBranchOfficeId" });
            DropColumn("dbo.Services", "Description");
            DropColumn("dbo.Services", "Email");
            DropColumn("dbo.Services", "Logo");
            DropColumn("dbo.Services", "AppUserId");
            DropColumn("dbo.AppUsers", "CanAddSercvice");
            DropColumn("dbo.AppUsers", "CenMakeRezervation");
            DropColumn("dbo.AppUsers", "Picture");
            DropColumn("dbo.AppUsers", "BirthDate");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Reservations");
            DropTable("dbo.Ratings");
            DropTable("dbo.Pricelists");
            DropTable("dbo.Items");
            DropTable("dbo.Comments");
            DropTable("dbo.BranchOffices");
        }
    }
}
