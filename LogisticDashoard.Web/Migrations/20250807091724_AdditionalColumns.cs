using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Shipment_Processing_Remarks",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Actual_Status",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "No_Of_Days_Delayed_ETA_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "No_Of_Days_Delayed_ETA_ATB",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "No_Of_Days_Delayed_ETD_ATD",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Transit_Days_ATD_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Vessel_Status",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No_Of_Days_Delayed_ETA_ATA",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "No_Of_Days_Delayed_ETA_ATB",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "No_Of_Days_Delayed_ETD_ATD",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "Transit_Days_ATD_ATA",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.DropColumn(
                name: "Vessel_Status",
                table: "SeaFreightScheduleMonitoring");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Shipment_Processing_Remarks",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Actual_Status",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
