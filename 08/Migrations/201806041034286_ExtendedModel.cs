namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Email", c => c.String());
            AddColumn("dbo.AppUsers", "BirthDay", c => c.DateTime());
            AddColumn("dbo.AppUsers", "Activated", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "PersonalDocument", c => c.String());
            AddColumn("dbo.Services", "Logo", c => c.String());
            AddColumn("dbo.Services", "Email", c => c.String());
            AddColumn("dbo.Services", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Description");
            DropColumn("dbo.Services", "Email");
            DropColumn("dbo.Services", "Logo");
            DropColumn("dbo.AppUsers", "PersonalDocument");
            DropColumn("dbo.AppUsers", "Activated");
            DropColumn("dbo.AppUsers", "BirthDay");
            DropColumn("dbo.AppUsers", "Email");
        }
    }
}
