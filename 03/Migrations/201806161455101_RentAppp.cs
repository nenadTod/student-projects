namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentAppp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "BranchOfficeId", "dbo.BranchOffices");
            DropForeignKey("dbo.Reservations", "ReturnBranchId", "dbo.BranchOffices");
            DropIndex("dbo.Vehicles", new[] { "BranchOfficeId" });
            DropIndex("dbo.Reservations", new[] { "ReturnBranchId" });
            AddColumn("dbo.Vehicles", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "GetBranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "ServiceId");
            CreateIndex("dbo.Reservations", "GetBranchId");
            AddForeignKey("dbo.Vehicles", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reservations", "GetBranchId", "dbo.BranchOffices", "BranchOfficeID", cascadeDelete: true);
            DropColumn("dbo.Vehicles", "BranchOfficeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "BranchOfficeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Reservations", "GetBranchId", "dbo.BranchOffices");
            DropForeignKey("dbo.Vehicles", "ServiceId", "dbo.Services");
            DropIndex("dbo.Reservations", new[] { "GetBranchId" });
            DropIndex("dbo.Vehicles", new[] { "ServiceId" });
            DropColumn("dbo.Reservations", "GetBranchId");
            DropColumn("dbo.Vehicles", "ServiceId");
            CreateIndex("dbo.Reservations", "ReturnBranchId");
            CreateIndex("dbo.Vehicles", "BranchOfficeId");
            AddForeignKey("dbo.Reservations", "ReturnBranchId", "dbo.BranchOffices", "BranchOfficeID", cascadeDelete: true);
            AddForeignKey("dbo.Vehicles", "BranchOfficeId", "dbo.BranchOffices", "BranchOfficeID", cascadeDelete: true);
        }
    }
}
