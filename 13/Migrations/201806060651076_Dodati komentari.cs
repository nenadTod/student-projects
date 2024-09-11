namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodatikomentari : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(),
                        Text = c.String(),
                        Author_Id = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.Author_Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Service_Id);
            
            AddColumn("dbo.Services", "AverageGrade", c => c.Single(nullable: false));
            AddColumn("dbo.Services", "NumberOfGrades", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AppUsers");
            DropIndex("dbo.Comments", new[] { "Service_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropColumn("dbo.Services", "NumberOfGrades");
            DropColumn("dbo.Services", "AverageGrade");
            DropTable("dbo.Comments");
        }
    }
}
