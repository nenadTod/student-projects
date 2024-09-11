namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinishedDatabaseIHopeSoGodHelpMePlease : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "TypeId", "dbo.NotificationTypes");
            DropIndex("dbo.Notifications", new[] { "TypeId" });
            AddColumn("dbo.Notifications", "Text", c => c.String(nullable: false));
            DropColumn("dbo.Notifications", "EntityId");
            DropColumn("dbo.Notifications", "TypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "TypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "EntityId", c => c.Int(nullable: false));
            DropColumn("dbo.Notifications", "Text");
            CreateIndex("dbo.Notifications", "TypeId");
            AddForeignKey("dbo.Notifications", "TypeId", "dbo.NotificationTypes", "Id", cascadeDelete: true);
        }
    }
}
