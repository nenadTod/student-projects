namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovedinService : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Approved");
        }
    }
}
