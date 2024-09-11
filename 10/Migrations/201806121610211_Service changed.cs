namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Servicechanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Services", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "Email", c => c.String());
            AlterColumn("dbo.Services", "Name", c => c.String());
        }
    }
}
