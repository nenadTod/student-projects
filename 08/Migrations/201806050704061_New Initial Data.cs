namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewInitialData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Branches", "Vehicle_id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "Vehicle_id", "dbo.Vehicles");
            DropIndex("dbo.Branches", new[] { "Vehicle_id" });
            DropIndex("dbo.Vehicles", new[] { "Vehicle_id" });
            AddColumn("dbo.Branches", "Service_Id", c => c.Int());
            AddColumn("dbo.Vehicles", "Service_Id", c => c.Int());
            AlterColumn("dbo.Branches", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Branches", "Longitude", c => c.Double(nullable: false));
            CreateIndex("dbo.Branches", "Service_Id");
            CreateIndex("dbo.Vehicles", "Service_Id");
            AddForeignKey("dbo.Branches", "Service_Id", "dbo.Services", "Id");
            AddForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services", "Id");
            DropColumn("dbo.Branches", "Vehicle_id");
            DropColumn("dbo.Vehicles", "Vehicle_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Vehicle_id", c => c.Int());
            AddColumn("dbo.Branches", "Vehicle_id", c => c.Int());
            DropForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Branches", "Service_Id", "dbo.Services");
            DropIndex("dbo.Vehicles", new[] { "Service_Id" });
            DropIndex("dbo.Branches", new[] { "Service_Id" });
            AlterColumn("dbo.Branches", "Longitude", c => c.Long(nullable: false));
            AlterColumn("dbo.Branches", "Latitude", c => c.Long(nullable: false));
            DropColumn("dbo.Vehicles", "Service_Id");
            DropColumn("dbo.Branches", "Service_Id");
            CreateIndex("dbo.Vehicles", "Vehicle_id");
            CreateIndex("dbo.Branches", "Vehicle_id");
            AddForeignKey("dbo.Vehicles", "Vehicle_id", "dbo.Vehicles", "id");
            AddForeignKey("dbo.Branches", "Vehicle_id", "dbo.Vehicles", "id");
        }
    }
}
