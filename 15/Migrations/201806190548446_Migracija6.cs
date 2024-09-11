namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Pictures", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Pictures");
        }
    }
}
