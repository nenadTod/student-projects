namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteService : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "IsDeleted");
        }
    }
}
