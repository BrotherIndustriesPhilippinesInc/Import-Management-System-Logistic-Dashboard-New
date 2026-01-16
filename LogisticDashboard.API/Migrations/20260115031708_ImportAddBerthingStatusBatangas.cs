using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ImportAddBerthingStatusBatangas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportBerthingStatusBatangas",
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
                    table.PrimaryKey("PK_ImportBerthingStatusBatangas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportBerthingStatusBatangas");
        }
    }
}
