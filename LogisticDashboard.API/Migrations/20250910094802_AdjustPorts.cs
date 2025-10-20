using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AdjustPorts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Normal_Range_Overall_Yard_Utilization",
                table: "Ports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Normal_Range_Vessels_At_Anchorage",
                table: "Ports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Normal_Range_Vessels_At_Berth",
                table: "Ports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Normal_Range_Overall_Yard_Utilization",
                table: "Ports");

            migrationBuilder.DropColumn(
                name: "Normal_Range_Vessels_At_Anchorage",
                table: "Ports");

            migrationBuilder.DropColumn(
                name: "Normal_Range_Vessels_At_Berth",
                table: "Ports");
        }
    }
}
