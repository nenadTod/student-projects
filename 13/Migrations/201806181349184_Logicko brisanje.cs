namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logickobrisanje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Branches", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vehicles", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TypeOfVehicles", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "Deleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.AppUsers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Email", c => c.String());
            DropColumn("dbo.Services", "Deleted");
            DropColumn("dbo.Comments", "Deleted");
            DropColumn("dbo.TypeOfVehicles", "Deleted");
            DropColumn("dbo.Vehicles", "Deleted");
            DropColumn("dbo.Branches", "Deleted");
            DropColumn("dbo.Rents", "Deleted");
            DropColumn("dbo.AppUsers", "Deleted");
        }
    }
}
