using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDeliveryLeadtimeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Average",
                table: "DeliveryLeadtimeData",
                newName: "TargetFYMin");

            migrationBuilder.AddColumn<int>(
                name: "ActualFYAverage",
                table: "DeliveryLeadtimeData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfBLFY",
                table: "DeliveryLeadtimeData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetFYMax",
                table: "DeliveryLeadtimeData",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualFYAverage",
                table: "DeliveryLeadtimeData");

            migrationBuilder.DropColumn(
                name: "NoOfBLFY",
                table: "DeliveryLeadtimeData");

            migrationBuilder.DropColumn(
                name: "TargetFYMax",
                table: "DeliveryLeadtimeData");

            migrationBuilder.RenameColumn(
                name: "TargetFYMin",
                table: "DeliveryLeadtimeData",
                newName: "Average");
        }
    }
}
