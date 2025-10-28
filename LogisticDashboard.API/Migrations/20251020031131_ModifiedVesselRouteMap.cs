using System;
using LogisticDashboard.Core;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedVesselRouteMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VesselRouteMap",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DestinationPort",
                table: "VesselRouteMap",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Coordinates>(
                name: "DestinationPortCoordinates",
                table: "VesselRouteMap",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationPortName",
                table: "VesselRouteMap",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "VesselRouteMap",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginPort",
                table: "VesselRouteMap",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Coordinates>(
                name: "OriginPortCoordinates",
                table: "VesselRouteMap",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginPortName",
                table: "VesselRouteMap",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "DestinationPort",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "DestinationPortCoordinates",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "DestinationPortName",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "OriginPort",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "OriginPortCoordinates",
                table: "VesselRouteMap");

            migrationBuilder.DropColumn(
                name: "OriginPortName",
                table: "VesselRouteMap");
        }
    }
}
