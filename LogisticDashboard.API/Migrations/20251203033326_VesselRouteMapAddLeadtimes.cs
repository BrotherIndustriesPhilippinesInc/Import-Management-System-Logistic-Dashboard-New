using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class VesselRouteMapAddLeadtimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Import_Processing_Leadtime",
                table: "VesselRouteMap",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Origin_To_Destination_Port",
                table: "VesselRouteMap",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Total_Leadtime",
                table: "VesselRouteMap",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Import_Processing_Leadtime",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "Origin_To_Destination_Port",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "Total_Leadtime",
                table: "VesselRouteMap");
        }
    }
}
