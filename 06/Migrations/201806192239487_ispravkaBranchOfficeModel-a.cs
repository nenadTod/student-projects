namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ispravkaBranchOfficeModela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchOffices", "Longitude", c => c.Double(nullable: false));
            DropColumn("dbo.BranchOffices", "Longtitue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchOffices", "Longtitue", c => c.Double(nullable: false));
            DropColumn("dbo.BranchOffices", "Longitude");
        }
    }
}
