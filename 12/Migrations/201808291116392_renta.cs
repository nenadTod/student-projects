namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "Bill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "Bill");
        }
    }
}
