namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rents", new[] { "Vehicle_id" });
            AddColumn("dbo.Vehicles", "VehicleImagesBase", c => c.String());
            CreateIndex("dbo.Rents", "Vehicle_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rents", new[] { "Vehicle_Id" });
            DropColumn("dbo.Vehicles", "VehicleImagesBase");
            CreateIndex("dbo.Rents", "Vehicle_id");
        }
    }
}
