namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceblockiservicecreator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "ServiceBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "CreatorUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "CreatorUserName");
            DropColumn("dbo.AppUsers", "ServiceBlock");
        }
    }
}
