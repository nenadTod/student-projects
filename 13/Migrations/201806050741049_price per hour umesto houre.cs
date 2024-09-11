namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class priceperhourumestohoure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "PricePerHour", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Vehicles", "PricePerHoure");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "PricePerHoure", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Vehicles", "PricePerHour");
        }
    }
}
