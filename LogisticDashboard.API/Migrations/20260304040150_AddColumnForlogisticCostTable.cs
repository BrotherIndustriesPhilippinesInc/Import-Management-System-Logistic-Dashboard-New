using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnForlogisticCostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DS",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FS",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GoGreen",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Local",
                table: "LogisticCost",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DS",
                table: "LogisticCost");

            migrationBuilder.DropColumn(
                name: "FS",
                table: "LogisticCost");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "LogisticCost");

            migrationBuilder.DropColumn(
                name: "GoGreen",
                table: "LogisticCost");

            migrationBuilder.DropColumn(
                name: "Local",
                table: "LogisticCost");
        }
    }
}
