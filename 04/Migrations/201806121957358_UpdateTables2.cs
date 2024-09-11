namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "Type_Id", "dbo.VehicleTypes");
            DropIndex("dbo.Vehicles", new[] { "Type_Id" });
            RenameColumn(table: "dbo.Vehicles", name: "Type_Id", newName: "TypeId");
            AlterColumn("dbo.Vehicles", "TypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "TypeId");
            AddForeignKey("dbo.Vehicles", "TypeId", "dbo.VehicleTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "TypeId", "dbo.VehicleTypes");
            DropIndex("dbo.Vehicles", new[] { "TypeId" });
            AlterColumn("dbo.Vehicles", "TypeId", c => c.Int());
            RenameColumn(table: "dbo.Vehicles", name: "TypeId", newName: "Type_Id");
            CreateIndex("dbo.Vehicles", "Type_Id");
            AddForeignKey("dbo.Vehicles", "Type_Id", "dbo.VehicleTypes", "Id");
        }
    }
}
