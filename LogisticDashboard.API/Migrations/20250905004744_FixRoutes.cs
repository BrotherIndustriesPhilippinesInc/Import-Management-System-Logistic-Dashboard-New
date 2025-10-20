using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old DateTime column
            migrationBuilder.DropColumn(
                name: "FiscalYear",
                table: "Routes");

            // Add a new int column
            migrationBuilder.AddColumn<int>(
                name: "FiscalYear",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the int column
            migrationBuilder.DropColumn(
                name: "FiscalYear",
                table: "Routes");

            // Add back the DateTime column if rollback happens
            migrationBuilder.AddColumn<DateTime>(
                name: "FiscalYear",
                table: "Routes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");
        }
    }
}
