namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Rating", c => c.Double(nullable: false));
            DropColumn("dbo.Services", "Grade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Grade", c => c.Double(nullable: false));
            DropColumn("dbo.Services", "Rating");
        }
    }
}
