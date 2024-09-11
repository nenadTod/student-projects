namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class implementedinitialmodelrents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppUsers", "Rent_Id", "dbo.Rents");
            DropIndex("dbo.AppUsers", new[] { "Rent_Id" });
            AddColumn("dbo.Rents", "AppUser_Id", c => c.Int());
            CreateIndex("dbo.Rents", "AppUser_Id");
            AddForeignKey("dbo.Rents", "AppUser_Id", "dbo.AppUsers", "Id");
            DropColumn("dbo.AppUsers", "Rent_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Rent_Id", c => c.Int());
            DropForeignKey("dbo.Rents", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.Rents", new[] { "AppUser_Id" });
            DropColumn("dbo.Rents", "AppUser_Id");
            CreateIndex("dbo.AppUsers", "Rent_Id");
            AddForeignKey("dbo.AppUsers", "Rent_Id", "dbo.Rents", "Id");
        }
    }
}
