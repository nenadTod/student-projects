namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Check : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BranchOffices", "Service_Id", "dbo.Services");
            DropIndex("dbo.BranchOffices", new[] { "Service_Id" });
            RenameColumn(table: "dbo.BranchOffices", name: "Service_Id", newName: "ServiceId");
            AlterColumn("dbo.BranchOffices", "ServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchOffices", "ServiceId");
            AddForeignKey("dbo.BranchOffices", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchOffices", "ServiceId", "dbo.Services");
            DropIndex("dbo.BranchOffices", new[] { "ServiceId" });
            AlterColumn("dbo.BranchOffices", "ServiceId", c => c.Int());
            RenameColumn(table: "dbo.BranchOffices", name: "ServiceId", newName: "Service_Id");
            CreateIndex("dbo.BranchOffices", "Service_Id");
            AddForeignKey("dbo.BranchOffices", "Service_Id", "dbo.Services", "Id");
        }
    }
}
