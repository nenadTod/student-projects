namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracija5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "FullNameUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "FullNameUser");
        }
    }
}
