namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailandRoleinAppuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Email", c => c.String());
            AddColumn("dbo.AppUsers", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Role");
            DropColumn("dbo.AppUsers", "Email");
        }
    }
}
