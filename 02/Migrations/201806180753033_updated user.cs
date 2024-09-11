namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateduser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Manage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Manage");
        }
    }
}
