namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        DatePosted = c.DateTime(),
                        Comment = c.String(),
                        Stars = c.Int(nullable: false),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.Service_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Service_Id", "dbo.Services");
            DropIndex("dbo.Reviews", new[] { "Service_Id" });
            DropTable("dbo.Reviews");
        }
    }
}
