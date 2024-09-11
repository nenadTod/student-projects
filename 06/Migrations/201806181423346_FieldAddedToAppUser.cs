namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldAddedToAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Blocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Blocked");
        }
    }
}
