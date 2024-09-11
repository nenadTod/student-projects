namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedrent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "BeginBranch_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "EndBranch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "BeginBranch_Id" });
            DropIndex("dbo.Rents", new[] { "EndBranch_Id" });
            AddColumn("dbo.Rents", "BeginBranch", c => c.String());
            AddColumn("dbo.Rents", "EndBranch", c => c.String());
            DropColumn("dbo.Rents", "BeginBranch_Id");
            DropColumn("dbo.Rents", "EndBranch_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rents", "EndBranch_Id", c => c.Int());
            AddColumn("dbo.Rents", "BeginBranch_Id", c => c.Int());
            DropColumn("dbo.Rents", "EndBranch");
            DropColumn("dbo.Rents", "BeginBranch");
            CreateIndex("dbo.Rents", "EndBranch_Id");
            CreateIndex("dbo.Rents", "BeginBranch_Id");
            AddForeignKey("dbo.Rents", "EndBranch_Id", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "BeginBranch_Id", "dbo.Branches", "Id");
        }
    }
}
