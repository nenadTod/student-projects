namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        IsNegative = c.Boolean(nullable: false),
                        PostedDate = c.DateTime(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.AppUsers", "Activated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Activated", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AppUsers");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropTable("dbo.Comments");
        }
    }
}
