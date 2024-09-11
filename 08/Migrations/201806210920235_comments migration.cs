namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsmigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "AppUser_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_User_Id", newName: "IX_AppUser_Id");
            AddColumn("dbo.Comments", "UserKey", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Service_Id", c => c.Int());
            CreateIndex("dbo.Comments", "Service_Id");
            AddForeignKey("dbo.Comments", "Service_Id", "dbo.Services", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Service_Id", "dbo.Services");
            DropIndex("dbo.Comments", new[] { "Service_Id" });
            DropColumn("dbo.Comments", "Service_Id");
            DropColumn("dbo.Comments", "UserKey");
            RenameIndex(table: "dbo.Comments", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Comments", name: "AppUser_Id", newName: "User_Id");
        }
    }
}
