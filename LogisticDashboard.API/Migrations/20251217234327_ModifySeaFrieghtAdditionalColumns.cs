using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifySeaFrieghtAdditionalColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SeaFreightScheduleMonitoring",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SeaFreightScheduleMonitoring");
        }
    }
}
