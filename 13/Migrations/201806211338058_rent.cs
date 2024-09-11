namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "GetBranch_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "RetBranch_Id", "dbo.Branches");
            DropForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.Rents", new[] { "GetBranch_Id" });
            DropIndex("dbo.Rents", new[] { "RetBranch_Id" });
            DropIndex("dbo.Rents", new[] { "Vehicle_Id" });
            RenameColumn(table: "dbo.Rents", name: "GetBranch_Id", newName: "GetBranchId");
            RenameColumn(table: "dbo.Rents", name: "RetBranch_Id", newName: "RetBranchId");
            RenameColumn(table: "dbo.Rents", name: "Vehicle_Id", newName: "VehicleId");
            AlterColumn("dbo.Rents", "GetBranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.Rents", "RetBranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.Rents", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rents", "GetBranchId");
            CreateIndex("dbo.Rents", "RetBranchId");
            CreateIndex("dbo.Rents", "VehicleId");
            AddForeignKey("dbo.Rents", "GetBranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "RetBranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "VehicleId", "dbo.Vehicles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Rents", "RetBranchId", "dbo.Branches");
            DropForeignKey("dbo.Rents", "GetBranchId", "dbo.Branches");
            DropIndex("dbo.Rents", new[] { "VehicleId" });
            DropIndex("dbo.Rents", new[] { "RetBranchId" });
            DropIndex("dbo.Rents", new[] { "GetBranchId" });
            AlterColumn("dbo.Rents", "VehicleId", c => c.Int());
            AlterColumn("dbo.Rents", "RetBranchId", c => c.Int());
            AlterColumn("dbo.Rents", "GetBranchId", c => c.Int());
            RenameColumn(table: "dbo.Rents", name: "VehicleId", newName: "Vehicle_Id");
            RenameColumn(table: "dbo.Rents", name: "RetBranchId", newName: "RetBranch_Id");
            RenameColumn(table: "dbo.Rents", name: "GetBranchId", newName: "GetBranch_Id");
            CreateIndex("dbo.Rents", "Vehicle_Id");
            CreateIndex("dbo.Rents", "RetBranch_Id");
            CreateIndex("dbo.Rents", "GetBranch_Id");
            AddForeignKey("dbo.Rents", "Vehicle_Id", "dbo.Vehicles", "Id");
            AddForeignKey("dbo.Rents", "RetBranch_Id", "dbo.Branches", "Id");
            AddForeignKey("dbo.Rents", "GetBranch_Id", "dbo.Branches", "Id");
        }
    }
}
