namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaaaaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "DateOfBirth", c => c.DateTime());
            DropColumn("dbo.AppUsers", "Birthday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Birthday", c => c.DateTime());
            DropColumn("dbo.AppUsers", "DateOfBirth");
        }
    }
}
