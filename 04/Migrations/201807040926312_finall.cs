namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finall : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Payed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Payed");
        }
    }
}
