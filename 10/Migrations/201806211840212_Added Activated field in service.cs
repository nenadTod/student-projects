namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActivatedfieldinservice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Activated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Activated");
        }
    }
}
