namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pricelists", "PricelistService_Id", "dbo.Services");
            DropIndex("dbo.Pricelists", new[] { "PricelistService_Id" });
            RenameColumn(table: "dbo.Pricelists", name: "PricelistService_Id", newName: "PricelistServiceId");
            AlterColumn("dbo.Pricelists", "PricelistServiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pricelists", "PricelistServiceId");
            AddForeignKey("dbo.Pricelists", "PricelistServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pricelists", "PricelistServiceId", "dbo.Services");
            DropIndex("dbo.Pricelists", new[] { "PricelistServiceId" });
            AlterColumn("dbo.Pricelists", "PricelistServiceId", c => c.Int());
            RenameColumn(table: "dbo.Pricelists", name: "PricelistServiceId", newName: "PricelistService_Id");
            CreateIndex("dbo.Pricelists", "PricelistService_Id");
            AddForeignKey("dbo.Pricelists", "PricelistService_Id", "dbo.Services", "Id");
        }
    }
}
