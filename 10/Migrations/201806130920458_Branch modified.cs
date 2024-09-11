namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Branchmodified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Branches", "Address", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Branches", "Address", c => c.String());
        }
    }
}
