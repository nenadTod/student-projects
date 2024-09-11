namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "PricePerHour", c => c.Double());
            AddColumn("dbo.Vehicles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Discriminator");
            DropColumn("dbo.Vehicles", "PricePerHour");
        }
    }
}
