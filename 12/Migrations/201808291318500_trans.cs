namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderID = c.String(),
                        PaymentID = c.String(),
                        PayerID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Rents", "Bill");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rents", "Bill", c => c.String());
            DropTable("dbo.Transactions");
        }
    }
}
