namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Image", c => c.String());
            DropColumn("dbo.Vehicles", "Images");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Images", c => c.String());
            DropColumn("dbo.Vehicles", "Image");
        }
    }
}
