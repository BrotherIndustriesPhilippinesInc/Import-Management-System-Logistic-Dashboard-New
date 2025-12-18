using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifySeaFrieghtAdditionalColumnsUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SeaFreightScheduleMonitoring",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SeaFreightScheduleMonitoring");
        }
    }
}
