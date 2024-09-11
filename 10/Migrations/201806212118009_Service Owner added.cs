namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceOwneradded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Owner");
        }
    }
}
