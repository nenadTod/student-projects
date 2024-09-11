namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menjanje : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Branches", name: "Service_Id", newName: "Services_Id");
            RenameColumn(table: "dbo.Vehicles", name: "Service_Id", newName: "Services_Id");
            RenameIndex(table: "dbo.Branches", name: "IX_Service_Id", newName: "IX_Services_Id");
            RenameIndex(table: "dbo.Vehicles", name: "IX_Service_Id", newName: "IX_Services_Id");
            AddColumn("dbo.Services", "Owner", c => c.String());
            AddColumn("dbo.Services", "Available", c => c.Boolean(nullable: false));
            DropColumn("dbo.Branches", "Name");
            DropColumn("dbo.Vehicles", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Image", c => c.String());
            AddColumn("dbo.Branches", "Name", c => c.String());
            DropColumn("dbo.Services", "Available");
            DropColumn("dbo.Services", "Owner");
            RenameIndex(table: "dbo.Vehicles", name: "IX_Services_Id", newName: "IX_Service_Id");
            RenameIndex(table: "dbo.Branches", name: "IX_Services_Id", newName: "IX_Service_Id");
            RenameColumn(table: "dbo.Vehicles", name: "Services_Id", newName: "Service_Id");
            RenameColumn(table: "dbo.Branches", name: "Services_Id", newName: "Service_Id");
        }
    }
}
