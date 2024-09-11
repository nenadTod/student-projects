namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "Used", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "Used");
        }
    }
}
