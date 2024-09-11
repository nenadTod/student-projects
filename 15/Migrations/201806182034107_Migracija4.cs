namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Rating");
        }
    }
}
