namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finmallll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "PaymentId", c => c.String());
            DropColumn("dbo.Reservations", "PaymendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "PaymendId", c => c.String());
            DropColumn("dbo.Reservations", "PaymentId");
        }
    }
}
