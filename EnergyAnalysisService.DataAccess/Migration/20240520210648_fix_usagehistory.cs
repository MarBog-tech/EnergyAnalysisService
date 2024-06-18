using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyAnalysisService.DataAccess.Migration
{
    /// <inheritdoc />
    public partial class fix_PaymentSlip : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TimeOfUse",
                table: "UsageHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfUse",
                table: "UsageHistories");
        }
    }
}
