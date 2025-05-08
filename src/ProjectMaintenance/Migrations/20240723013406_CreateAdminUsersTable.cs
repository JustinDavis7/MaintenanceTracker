using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspnetIdentityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LeadOperator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AcquiredDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WarrantyExpiration = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__3214EC077BE66853", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC078D059C4B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    TicketCreatorId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PriorityLevel = table.Column<int>(type: "int", nullable: false),
                    RequestCreationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Satisfied = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    PlannedCompletion = table.Column<DateOnly>(type: "date", nullable: false),
                    PartsList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriorityBump = table.Column<bool>(type: "bit", nullable: false),
                    AssignedWorker = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__3214EC071DD195AD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_Equpiment",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_User",
                        column: x => x.TicketCreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_EquipmentId",
                table: "MaintenanceTicket",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_TicketCreatorId",
                table: "MaintenanceTicket",
                column: "TicketCreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "MaintenanceTicket");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
