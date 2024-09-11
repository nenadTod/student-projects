namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Servicemodelchanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Rating");
        }
    }
}
