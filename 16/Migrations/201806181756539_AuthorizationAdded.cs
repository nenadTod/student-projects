namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorizationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Authorized", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Authorized");
        }
    }
}
