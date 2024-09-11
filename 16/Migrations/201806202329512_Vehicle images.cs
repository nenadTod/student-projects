namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vehicleimages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Images", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Images");
        }
    }
}
