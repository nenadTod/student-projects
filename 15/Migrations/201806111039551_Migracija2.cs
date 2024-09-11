namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "CanMakeReservation", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vehicles", "Producer", c => c.String());
            DropColumn("dbo.AppUsers", "CenMakeRezervation");
            DropColumn("dbo.Vehicles", "Produce");
            DropColumn("dbo.Vehicles", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.Vehicles", "Produce", c => c.String());
            AddColumn("dbo.AppUsers", "CenMakeRezervation", c => c.Boolean(nullable: false));
            DropColumn("dbo.Vehicles", "Producer");
            DropColumn("dbo.AppUsers", "CanMakeReservation");
        }
    }
}
