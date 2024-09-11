namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedManagerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ManagerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "ManagerId");
        }
    }
}
