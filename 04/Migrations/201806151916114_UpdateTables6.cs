namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pricelists", "PricelistServiceId", "dbo.Services");
            DropIndex("dbo.Pricelists", new[] { "PricelistServiceId" });
            RenameColumn(table: "dbo.Pricelists", name: "PricelistServiceId", newName: "PricelistService_Id");
            AlterColumn("dbo.Pricelists", "PricelistService_Id", c => c.Int());
            CreateIndex("dbo.Pricelists", "PricelistService_Id");
            AddForeignKey("dbo.Pricelists", "PricelistService_Id", "dbo.Services", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pricelists", "PricelistService_Id", "dbo.Services");
            DropIndex("dbo.Pricelists", new[] { "PricelistService_Id" });
            AlterColumn("dbo.Pricelists", "PricelistService_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Pricelists", name: "PricelistService_Id", newName: "PricelistServiceId");
            CreateIndex("dbo.Pricelists", "PricelistServiceId");
            AddForeignKey("dbo.Pricelists", "PricelistServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
    }
}
