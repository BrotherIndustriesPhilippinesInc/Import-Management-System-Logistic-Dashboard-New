using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalDetailsAirFreight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AirFreightScheduleMonitoring",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AirFreightScheduleMonitoring",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AirFreightScheduleMonitoring",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AirFreightScheduleMonitoring",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AirFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AirFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AirFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AirFreightScheduleMonitoring");
        }
    }
}
