namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija71 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Pirce", c => c.Double(nullable: false));
            DropColumn("dbo.Vehicles", "Available");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Available", c => c.Boolean(nullable: false));
            DropColumn("dbo.Vehicles", "Pirce");
        }
    }
}
