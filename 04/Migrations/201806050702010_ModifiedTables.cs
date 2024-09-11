namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AppUsers");
            DropIndex("dbo.Notifications", new[] { "UserId" });
            RenameColumn(table: "dbo.Notifications", name: "UserId", newName: "AppUser_Id");
            AddColumn("dbo.Notifications", "EntityId", c => c.Int(nullable: false));
            AddColumn("dbo.VehicleTypes", "Service_Id", c => c.Int());
            AlterColumn("dbo.AppUsers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.AppUsers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Notifications", "AppUser_Id", c => c.Int());
            AlterColumn("dbo.NotificationTypes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Branches", "Image", c => c.String(nullable: false));
            AlterColumn("dbo.Branches", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Services", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Services", "LogoImagePath", c => c.String(nullable: false));
            AlterColumn("dbo.Services", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Services", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Comments", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Vehicles", "Model", c => c.String(nullable: false));
            AlterColumn("dbo.Vehicles", "Manufacturer", c => c.String(nullable: false));
            AlterColumn("dbo.Vehicles", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.VehicleImages", "ImagePath", c => c.String(nullable: false));
            AlterColumn("dbo.VehicleTypes", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.VehicleTypes", "Service_Id");
            CreateIndex("dbo.Notifications", "AppUser_Id");
            AddForeignKey("dbo.VehicleTypes", "Service_Id", "dbo.Services", "Id");
            AddForeignKey("dbo.Notifications", "AppUser_Id", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.VehicleTypes", "Service_Id", "dbo.Services");
            DropIndex("dbo.Notifications", new[] { "AppUser_Id" });
            DropIndex("dbo.VehicleTypes", new[] { "Service_Id" });
            AlterColumn("dbo.VehicleTypes", "Name", c => c.String());
            AlterColumn("dbo.VehicleImages", "ImagePath", c => c.String());
            AlterColumn("dbo.Vehicles", "Description", c => c.String());
            AlterColumn("dbo.Vehicles", "Manufacturer", c => c.String());
            AlterColumn("dbo.Vehicles", "Model", c => c.String());
            AlterColumn("dbo.Comments", "Content", c => c.String());
            AlterColumn("dbo.Services", "Description", c => c.String());
            AlterColumn("dbo.Services", "Email", c => c.String());
            AlterColumn("dbo.Services", "LogoImagePath", c => c.String());
            AlterColumn("dbo.Services", "Name", c => c.String());
            AlterColumn("dbo.Branches", "Address", c => c.String());
            AlterColumn("dbo.Branches", "Image", c => c.String());
            AlterColumn("dbo.NotificationTypes", "Name", c => c.String());
            AlterColumn("dbo.Notifications", "AppUser_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "LastName", c => c.String());
            AlterColumn("dbo.AppUsers", "FullName", c => c.String());
            DropColumn("dbo.VehicleTypes", "Service_Id");
            DropColumn("dbo.Notifications", "EntityId");
            RenameColumn(table: "dbo.Notifications", name: "AppUser_Id", newName: "UserId");
            CreateIndex("dbo.Notifications", "UserId");
            AddForeignKey("dbo.Notifications", "UserId", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
