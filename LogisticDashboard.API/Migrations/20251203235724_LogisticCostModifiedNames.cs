using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class LogisticCostModifiedNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "LogisticCost");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LogisticCost",
                newName: "KGS");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalUSD",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalUSD",
                table: "LogisticCost");

            migrationBuilder.RenameColumn(
                name: "KGS",
                table: "LogisticCost",
                newName: "Name");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "LogisticCost",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
