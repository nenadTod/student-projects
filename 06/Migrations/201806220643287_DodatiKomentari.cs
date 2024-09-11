namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatiKomentari : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RAIdentityUserId = c.String(maxLength: 128),
                        ServiceId = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RAIdentityUserId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.RAIdentityUserId)
                .Index(t => t.ServiceId);
            
            AddColumn("dbo.Services", "Rate", c => c.Single(nullable: false));
            AddColumn("dbo.Services", "NumOfRates", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Comments", "RAIdentityUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "ServiceId" });
            DropIndex("dbo.Comments", new[] { "RAIdentityUserId" });
            DropColumn("dbo.Services", "NumOfRates");
            DropColumn("dbo.Services", "Rate");
            DropTable("dbo.Comments");
        }
    }
}
