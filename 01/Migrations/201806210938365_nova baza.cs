namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class novabaza : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        Birthday = c.DateTime(),
                        PersonalDocument = c.String(),
                        Activated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        ReturnBranchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Branches", t => t.ReturnBranchId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VehicleId)
                .Index(t => t.ReturnBranchId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Logo = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatorId = c.Int(),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.CreatorId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.CreatorId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Logo = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Activated = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Model = c.String(nullable: false),
                        Manufactor = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        PricePerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unavailable = c.Boolean(nullable: false),
                        Images = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        TypeId = c.Int(nullable: false),
                        CreatorId = c.Int(),
                        BranchId = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.AppUsers", t => t.CreatorId)
                .ForeignKey("dbo.TypeOfVehicles", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.TypeId)
                .Index(t => t.CreatorId)
                .Index(t => t.BranchId)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.TypeOfVehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GradeNum = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        AppUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AppUserId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Grades", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Grades", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Comments", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Comments", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Rents", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Rents", "ReturnBranchId", "dbo.Branches");
            DropForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Vehicles", "TypeId", "dbo.TypeOfVehicles");
            DropForeignKey("dbo.Vehicles", "CreatorId", "dbo.AppUsers");
            DropForeignKey("dbo.Vehicles", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Services", "CreatorId", "dbo.AppUsers");
            DropForeignKey("dbo.Branches", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Branches", "CreatorId", "dbo.AppUsers");
            DropForeignKey("dbo.Rents", "UserId", "dbo.AppUsers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "AppUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Grades", new[] { "AppUserId" });
            DropIndex("dbo.Grades", new[] { "ServiceId" });
            DropIndex("dbo.Comments", new[] { "AppUserId" });
            DropIndex("dbo.Comments", new[] { "ServiceId" });
            DropIndex("dbo.Vehicles", new[] { "Service_Id" });
            DropIndex("dbo.Vehicles", new[] { "BranchId" });
            DropIndex("dbo.Vehicles", new[] { "CreatorId" });
            DropIndex("dbo.Vehicles", new[] { "TypeId" });
            DropIndex("dbo.Services", new[] { "CreatorId" });
            DropIndex("dbo.Branches", new[] { "ServiceId" });
            DropIndex("dbo.Branches", new[] { "CreatorId" });
            DropIndex("dbo.Rents", new[] { "ReturnBranchId" });
            DropIndex("dbo.Rents", new[] { "VehicleId" });
            DropIndex("dbo.Rents", new[] { "UserId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.Grades");
            DropTable("dbo.Comments");
            DropTable("dbo.TypeOfVehicles");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Services");
            DropTable("dbo.Branches");
            DropTable("dbo.Rents");
            DropTable("dbo.AppUsers");
        }
    }
}
