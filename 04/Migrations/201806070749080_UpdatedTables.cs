namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VehicleTypes", "Service_Id", "dbo.Services");
            DropIndex("dbo.VehicleTypes", new[] { "Service_Id" });
            DropColumn("dbo.VehicleTypes", "Service_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VehicleTypes", "Service_Id", c => c.Int());
            CreateIndex("dbo.VehicleTypes", "Service_Id");
            AddForeignKey("dbo.VehicleTypes", "Service_Id", "dbo.Services", "Id");
        }
    }
}
