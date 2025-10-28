using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class FixVesselRouteMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationPort",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "OriginPort",
                table: "VesselRouteMap");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationPort",
                table: "VesselRouteMap",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginPort",
                table: "VesselRouteMap",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
