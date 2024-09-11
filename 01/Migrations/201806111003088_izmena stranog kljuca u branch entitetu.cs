namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class izmenastranogkljucaubranchentitetu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Branches", "Service_Id", "dbo.Services");
            DropIndex("dbo.Branches", new[] { "Service_Id" });
            RenameColumn(table: "dbo.Branches", name: "Service_Id", newName: "ServiceId");
            AlterColumn("dbo.Branches", "ServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Branches", "ServiceId");
            AddForeignKey("dbo.Branches", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "ServiceId", "dbo.Services");
            DropIndex("dbo.Branches", new[] { "ServiceId" });
            AlterColumn("dbo.Branches", "ServiceId", c => c.Int());
            RenameColumn(table: "dbo.Branches", name: "ServiceId", newName: "Service_Id");
            CreateIndex("dbo.Branches", "Service_Id");
            AddForeignKey("dbo.Branches", "Service_Id", "dbo.Services", "Id");
        }
    }
}
