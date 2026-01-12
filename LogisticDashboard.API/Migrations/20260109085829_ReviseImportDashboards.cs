using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ReviseImportDashboards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportBerthingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortId = table.Column<int>(type: "integer", nullable: false),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Revised_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBerthingStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportBerthingStatus_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportDashboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Revised_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDashboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportDeliveryDashboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Revised_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDeliveryDashboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportPortUtilization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortId = table.Column<int>(type: "integer", nullable: false),
                    BLNo = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Original_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Revised_ETA_Port = table.Column<string>(type: "text", nullable: false),
                    Reasons = table.Column<string>(type: "text", nullable: false),
                    BIPH_Action = table.Column<string>(type: "text", nullable: false),
                    Criteria = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false)
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
                name: "IX_ImportBerthingStatus_PortId",
                table: "ImportBerthingStatus",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportPortUtilization_PortId",
                table: "ImportPortUtilization",
                column: "PortId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportBerthingStatus");

            migrationBuilder.DropTable(
                name: "ImportDashboards");

            migrationBuilder.DropTable(
                name: "ImportDeliveryDashboards");

            migrationBuilder.DropTable(
                name: "ImportPortUtilization");
        }
    }
}
