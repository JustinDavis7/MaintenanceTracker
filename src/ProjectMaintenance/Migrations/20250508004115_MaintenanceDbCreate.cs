using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceDbCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__User__3214EC078D059C4B",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Maintena__3214EC071DD195AD",
                table: "MaintenanceTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Equipmen__3214EC077BE66853",
                table: "Equipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK__User__3214EC07D49ADB05",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Maintena__3214EC07CCF9430B",
                table: "MaintenanceTicket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Equipmen__3214EC0707EB0615",
                table: "Equipment",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PMTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    DatePerformed = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PMTicket__3214EC07898DA921", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMTicket_Equpiment",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PMTicket_EquipmentId",
                table: "PMTicket",
                column: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK__User__3214EC07D49ADB05",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Maintena__3214EC07CCF9430B",
                table: "MaintenanceTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Equipmen__3214EC0707EB0615",
                table: "Equipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK__User__3214EC078D059C4B",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Maintena__3214EC071DD195AD",
                table: "MaintenanceTicket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Equipmen__3214EC077BE66853",
                table: "Equipment",
                column: "Id");
        }
    }
}
