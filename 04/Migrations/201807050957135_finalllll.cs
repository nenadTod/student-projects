namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalllll : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "OrderId", c => c.String());
        }
    }
}
