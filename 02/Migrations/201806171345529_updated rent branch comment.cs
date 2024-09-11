namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedrentbranchcomment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Branches", new[] { "service_Id" });
            RenameColumn(table: "dbo.Rents", name: "AppUser_Id", newName: "User_Id");
            RenameIndex(table: "dbo.Rents", name: "IX_AppUser_Id", newName: "IX_User_Id");
            AddColumn("dbo.Comments", "User_Id", c => c.Int());
            CreateIndex("dbo.Branches", "Service_Id");
            CreateIndex("dbo.Comments", "User_Id");
            AddForeignKey("dbo.Comments", "User_Id", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AppUsers");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Branches", new[] { "Service_Id" });
            DropColumn("dbo.Comments", "User_Id");
            RenameIndex(table: "dbo.Rents", name: "IX_User_Id", newName: "IX_AppUser_Id");
            RenameColumn(table: "dbo.Rents", name: "User_Id", newName: "AppUser_Id");
            CreateIndex("dbo.Branches", "service_Id");
        }
    }
}
