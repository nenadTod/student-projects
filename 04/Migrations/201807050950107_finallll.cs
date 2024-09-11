namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finallll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "OrderId", c => c.String());
            AddColumn("dbo.Reservations", "PaymentId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "PaymentId");
            DropColumn("dbo.Reservations", "OrderId");
        }
    }
}
