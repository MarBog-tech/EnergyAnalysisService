using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyAnalysisService.DataAccess.Migration
{
    /// <inheritdoc />
    public partial class add_new_entity : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceForElectricity = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 2.64m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KilowattPerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KilowattPerMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KilowattPerYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageHistories_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsageHistories_DeviceId",
                table: "UsageHistories",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UsageHistories");
        }
    }
}
