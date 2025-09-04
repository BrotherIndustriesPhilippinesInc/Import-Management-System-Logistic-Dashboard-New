using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutesAndSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteName = table.Column<string>(type: "text", nullable: false),
                    From = table.Column<string>(type: "text", nullable: false),
                    To = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });
            
            migrationBuilder.CreateTable(
                name: "SailingSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Week = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VesselName = table.Column<int>(type: "integer", nullable: false),
                    VoyNo = table.Column<string>(type: "text", nullable: false),
                    Origin = table.Column<string>(type: "text", nullable: false),
                    OriginalETD = table.Column<string>(type: "text", nullable: false),
                    OriginalETAMNL = table.Column<string>(type: "text", nullable: false),
                    LatestETD = table.Column<string>(type: "text", nullable: false),
                    LatestETAMNL = table.Column<string>(type: "text", nullable: false),
                    TransitDays = table.Column<string>(type: "text", nullable: false),
                    DelayDeparture = table.Column<int>(type: "integer", nullable: false),
                    DelayArrival = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    RouteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SailingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SailingSchedule_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SailingSchedule_RouteId",
                table: "SailingSchedule",
                column: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "SailingSchedule");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
