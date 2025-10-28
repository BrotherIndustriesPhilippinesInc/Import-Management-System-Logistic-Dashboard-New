using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDeliveryLeadtimeDataYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FY",
                table: "DeliveryLeadtimeData",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "DeliveryLeadtime",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FY",
                table: "DeliveryLeadtimeData");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "DeliveryLeadtime",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
