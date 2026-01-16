using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ImportReviseDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportPortUtilization");

            migrationBuilder.CreateTable(
                name: "ImportPortUtilizationManila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revised_ETA_Port = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportPortUtilizationManila", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportPortUtilizationManila");

            migrationBuilder.CreateTable(
                name: "ImportPortUtilization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortId = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: true),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    Revised_ETA_Port = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportPortUtilization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportPortUtilization_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportPortUtilization_PortId",
                table: "ImportPortUtilization",
                column: "PortId");
        }
    }
}
