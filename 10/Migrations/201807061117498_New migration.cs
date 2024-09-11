namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeOfVote = c.String(),
                        Service_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.Service_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Rents", "BranchStart_Id", c => c.Int());
            CreateIndex("dbo.Rents", "BranchStart_Id");
            AddForeignKey("dbo.Rents", "BranchStart_Id", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "BranchStart_Id", "dbo.Branches");
            DropForeignKey("dbo.Ratings", "User_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Ratings", "Service_Id", "dbo.Services");
            DropIndex("dbo.Rents", new[] { "BranchStart_Id" });
            DropIndex("dbo.Ratings", new[] { "User_Id" });
            DropIndex("dbo.Ratings", new[] { "Service_Id" });
            DropColumn("dbo.Rents", "BranchStart_Id");
            DropTable("dbo.Ratings");
        }
    }
}
