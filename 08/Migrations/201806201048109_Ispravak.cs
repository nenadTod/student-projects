namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ispravak : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "IsProcessed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "ReturnBranch_Id", c => c.Int());
            AddColumn("dbo.Vehicles", "Images", c => c.String());
            AddColumn("dbo.Services", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "IsProcessed", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Rents", "ReturnBranch_Id");
            AddForeignKey("dbo.Rents", "ReturnBranch_Id", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "ReturnBranch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "ReturnBranch_Id" });
            DropColumn("dbo.Services", "IsProcessed");
            DropColumn("dbo.Services", "IsAccepted");
            DropColumn("dbo.Services", "IsDeleted");
            DropColumn("dbo.Vehicles", "Images");
            DropColumn("dbo.Rents", "ReturnBranch_Id");
            DropColumn("dbo.AppUsers", "IsProcessed");
            DropColumn("dbo.AppUsers", "IsAccepted");
        }
    }
}
