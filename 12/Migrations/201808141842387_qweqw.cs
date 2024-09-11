namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qweqw : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UserEmail", c => c.String());
            AddColumn("dbo.Comments", "ServiceName", c => c.String());
            AddColumn("dbo.Services", "Grade", c => c.Double(nullable: false));
            DropColumn("dbo.Vehicles", "Images");
            DropColumn("dbo.Comments", "User_Id");
            DropColumn("dbo.Comments", "Service_Id");
            DropColumn("dbo.Services", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Rating", c => c.Double(nullable: false));
            AddColumn("dbo.Comments", "Service_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "User_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "Images", c => c.String());
            DropColumn("dbo.Services", "Grade");
            DropColumn("dbo.Comments", "ServiceName");
            DropColumn("dbo.Comments", "UserEmail");
        }
    }
}
