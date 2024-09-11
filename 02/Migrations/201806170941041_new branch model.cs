namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newbranchmodel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Branches", new[] { "Service_Id" });
            CreateIndex("dbo.Branches", "service_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Branches", new[] { "service_Id" });
            CreateIndex("dbo.Branches", "Service_Id");
        }
    }
}
