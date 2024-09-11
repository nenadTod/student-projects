namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentaddedtomodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Grade = c.Int(nullable: false),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.Service_Id);
            
            AddColumn("dbo.Services", "Activated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Service_Id", "dbo.Services");
            DropIndex("dbo.Comments", new[] { "Service_Id" });
            DropColumn("dbo.Services", "Activated");
            DropTable("dbo.Comments");
        }
    }
}
