namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_Model2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "Manager_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Reservations", "ReturnBranchOffice_Id", "dbo.BranchOffices");
            DropForeignKey("dbo.Reservations", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Reservations", "TakeAwayBranchOffice_Id", "dbo.BranchOffices");
            DropForeignKey("dbo.Reservations", "User_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Reservations", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.Services", new[] { "Manager_Id" });
            DropIndex("dbo.Vehicles", new[] { "Service_Id" });
            DropIndex("dbo.Reservations", new[] { "ReturnBranchOffice_Id" });
            DropIndex("dbo.Reservations", new[] { "Service_Id" });
            DropIndex("dbo.Reservations", new[] { "TakeAwayBranchOffice_Id" });
            DropIndex("dbo.Reservations", new[] { "User_Id" });
            DropIndex("dbo.Reservations", new[] { "Vehicle_Id" });
            RenameColumn(table: "dbo.Services", name: "Manager_Id", newName: "ManagerId");
            RenameColumn(table: "dbo.Vehicles", name: "Service_Id", newName: "ServiceId");
            RenameColumn(table: "dbo.Reservations", name: "ReturnBranchOffice_Id", newName: "ReturnBranchOfficeId");
            RenameColumn(table: "dbo.Reservations", name: "Service_Id", newName: "ServiceId");
            RenameColumn(table: "dbo.Reservations", name: "TakeAwayBranchOffice_Id", newName: "TakeAwayBranchOfficeId");
            RenameColumn(table: "dbo.Reservations", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Reservations", name: "Vehicle_Id", newName: "VehicleId");
            AlterColumn("dbo.Services", "ManagerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Vehicles", "ServiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "ReturnBranchOfficeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "ServiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "TakeAwayBranchOfficeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "ManagerId");
            CreateIndex("dbo.Vehicles", "ServiceId");
            CreateIndex("dbo.Reservations", "UserId");
            CreateIndex("dbo.Reservations", "VehicleId");
            CreateIndex("dbo.Reservations", "ServiceId");
            CreateIndex("dbo.Reservations", "ReturnBranchOfficeId");
            CreateIndex("dbo.Reservations", "TakeAwayBranchOfficeId");
            AddForeignKey("dbo.Services", "ManagerId", "dbo.AppUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Vehicles", "ServiceId", "dbo.Services", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservations", "ReturnBranchOfficeId", "dbo.BranchOffices", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservations", "ServiceId", "dbo.Services", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservations", "TakeAwayBranchOfficeId", "dbo.BranchOffices", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservations", "UserId", "dbo.AppUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservations", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: false);
            DropColumn("dbo.AppUsers", "Username");
            DropColumn("dbo.AppUsers", "Password");
            DropColumn("dbo.AppUsers", "Email");
            DropColumn("dbo.AppUsers", "UserType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "UserType", c => c.Int(nullable: false));
            AddColumn("dbo.AppUsers", "Email", c => c.String());
            AddColumn("dbo.AppUsers", "Password", c => c.String());
            AddColumn("dbo.AppUsers", "Username", c => c.String());
            DropForeignKey("dbo.Reservations", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Reservations", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.Reservations", "TakeAwayBranchOfficeId", "dbo.BranchOffices");
            DropForeignKey("dbo.Reservations", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Reservations", "ReturnBranchOfficeId", "dbo.BranchOffices");
            DropForeignKey("dbo.Vehicles", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "ManagerId", "dbo.AppUsers");
            DropIndex("dbo.Reservations", new[] { "TakeAwayBranchOfficeId" });
            DropIndex("dbo.Reservations", new[] { "ReturnBranchOfficeId" });
            DropIndex("dbo.Reservations", new[] { "ServiceId" });
            DropIndex("dbo.Reservations", new[] { "VehicleId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Vehicles", new[] { "ServiceId" });
            DropIndex("dbo.Services", new[] { "ManagerId" });
            AlterColumn("dbo.Reservations", "VehicleId", c => c.Int());
            AlterColumn("dbo.Reservations", "UserId", c => c.Int());
            AlterColumn("dbo.Reservations", "TakeAwayBranchOfficeId", c => c.Int());
            AlterColumn("dbo.Reservations", "ServiceId", c => c.Int());
            AlterColumn("dbo.Reservations", "ReturnBranchOfficeId", c => c.Int());
            AlterColumn("dbo.Vehicles", "ServiceId", c => c.Int());
            AlterColumn("dbo.Services", "ManagerId", c => c.Int());
            RenameColumn(table: "dbo.Reservations", name: "VehicleId", newName: "Vehicle_Id");
            RenameColumn(table: "dbo.Reservations", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Reservations", name: "TakeAwayBranchOfficeId", newName: "TakeAwayBranchOffice_Id");
            RenameColumn(table: "dbo.Reservations", name: "ServiceId", newName: "Service_Id");
            RenameColumn(table: "dbo.Reservations", name: "ReturnBranchOfficeId", newName: "ReturnBranchOffice_Id");
            RenameColumn(table: "dbo.Vehicles", name: "ServiceId", newName: "Service_Id");
            RenameColumn(table: "dbo.Services", name: "ManagerId", newName: "Manager_Id");
            CreateIndex("dbo.Reservations", "Vehicle_Id");
            CreateIndex("dbo.Reservations", "User_Id");
            CreateIndex("dbo.Reservations", "TakeAwayBranchOffice_Id");
            CreateIndex("dbo.Reservations", "Service_Id");
            CreateIndex("dbo.Reservations", "ReturnBranchOffice_Id");
            CreateIndex("dbo.Vehicles", "Service_Id");
            CreateIndex("dbo.Services", "Manager_Id");
            AddForeignKey("dbo.Reservations", "Vehicle_Id", "dbo.Vehicles", "Id");
            AddForeignKey("dbo.Reservations", "User_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Reservations", "TakeAwayBranchOffice_Id", "dbo.BranchOffices", "Id");
            AddForeignKey("dbo.Reservations", "Service_Id", "dbo.Services", "Id");
            AddForeignKey("dbo.Reservations", "ReturnBranchOffice_Id", "dbo.BranchOffices", "Id");
            AddForeignKey("dbo.Vehicles", "Service_Id", "dbo.Services", "Id");
            AddForeignKey("dbo.Services", "Manager_Id", "dbo.AppUsers", "Id");
        }
    }
}
