namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branch : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Rents", name: "Branch_Id", newName: "GetBranch_Id");
            RenameIndex(table: "dbo.Rents", name: "IX_Branch_Id", newName: "IX_GetBranch_Id");
            AddColumn("dbo.Rents", "RetBranch_Id", c => c.Int());
            CreateIndex("dbo.Rents", "RetBranch_Id");
            AddForeignKey("dbo.Rents", "RetBranch_Id", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "RetBranch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "RetBranch_Id" });
            DropColumn("dbo.Rents", "RetBranch_Id");
            RenameIndex(table: "dbo.Rents", name: "IX_GetBranch_Id", newName: "IX_Branch_Id");
            RenameColumn(table: "dbo.Rents", name: "GetBranch_Id", newName: "Branch_Id");
        }
    }
}
