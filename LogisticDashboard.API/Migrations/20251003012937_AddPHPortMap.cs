using LogisticDashboard.Core;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPHPortMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PHPortMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Carrier = table.Column<string>(type: "text", nullable: false),
                    SailingLeadtime = table.Column<string>(type: "text", nullable: false),
                    OriginPort = table.Column<string>(type: "text", nullable: false),
                    OriginPortCoordinates = table.Column<Coordinates>(type: "jsonb", nullable: false),
                    DestinationPort = table.Column<string>(type: "text", nullable: false),
                    DestinationPortCoordinates = table.Column<Coordinates>(type: "jsonb", nullable: false),
                    PictureLocation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHPortMap", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PHPortMap");
        }
    }
}
