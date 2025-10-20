using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIncoterms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incoterms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Seller = table.Column<string>(type: "text", nullable: false),
                    OriginTruckings = table.Column<string>(type: "text", nullable: false),
                    OriginCustoms = table.Column<string>(type: "text", nullable: false),
                    OriginTerminalCharges = table.Column<string>(type: "text", nullable: false),
                    OceanFreightAirFreight = table.Column<string>(type: "text", nullable: false),
                    DestinationTerminalCharges = table.Column<string>(type: "text", nullable: false),
                    DestinationCustoms = table.Column<string>(type: "text", nullable: false),
                    DestinationTrucking = table.Column<string>(type: "text", nullable: false),
                    Buyer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incoterms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incoterms");
        }
    }
}
