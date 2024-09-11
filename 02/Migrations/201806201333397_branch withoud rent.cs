namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branchwithoudrent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "Branch_Id" });
            DropColumn("dbo.Rents", "Branch_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rents", "Branch_Id", c => c.Int());
            CreateIndex("dbo.Rents", "Branch_Id");
            AddForeignKey("dbo.Rents", "Branch_Id", "dbo.Branches", "Id");
        }
    }
}
