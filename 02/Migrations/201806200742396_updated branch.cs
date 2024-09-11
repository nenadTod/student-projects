namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedbranch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "BeginBranch_Id", c => c.Int());
            AddColumn("dbo.Rents", "EndBranch_Id", c => c.Int());
            CreateIndex("dbo.Rents", "BeginBranch_Id");
            CreateIndex("dbo.Rents", "EndBranch_Id");
            AddForeignKey("dbo.Rents", "BeginBranch_Id", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "EndBranch_Id", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "EndBranch_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "BeginBranch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "EndBranch_Id" });
            DropIndex("dbo.Rents", new[] { "BeginBranch_Id" });
            DropColumn("dbo.Rents", "EndBranch_Id");
            DropColumn("dbo.Rents", "BeginBranch_Id");
        }
    }
}
