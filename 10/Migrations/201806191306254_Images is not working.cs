namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Imagesisnotworking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "VehicleImagesBase", c => c.String());
            DropColumn("dbo.Vehicles", "ImagesBase");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "ImagesBase", c => c.String());
            DropColumn("dbo.Vehicles", "VehicleImagesBase");
        }
    }
}
