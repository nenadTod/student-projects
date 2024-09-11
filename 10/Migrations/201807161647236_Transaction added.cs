namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transactionadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Rent_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rents", t => t.Rent_Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.Rent_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Rents", "Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "User_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Transactions", "Rent_Id", "dbo.Rents");
            DropIndex("dbo.Transactions", new[] { "User_Id" });
            DropIndex("dbo.Transactions", new[] { "Rent_Id" });
            DropColumn("dbo.Rents", "Paid");
            DropTable("dbo.Transactions");
        }
    }
}
