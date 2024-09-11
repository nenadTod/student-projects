namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vehicles", "PricePerHour");
            DropColumn("dbo.Vehicles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Vehicles", "PricePerHour", c => c.Double());
        }
    }
}
