namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Contact");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Contact", c => c.String());
        }
    }
}
