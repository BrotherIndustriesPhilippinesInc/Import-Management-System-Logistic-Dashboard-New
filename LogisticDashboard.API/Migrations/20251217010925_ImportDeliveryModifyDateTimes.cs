using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    /// <inheritdoc />
    public partial class ImportDeliveryModifyDateTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        ALTER TABLE "ImportDelivery"
        ALTER COLUMN "Original_ETA_Port"
        TYPE timestamp with time zone
        USING "Original_ETA_Port"::timestamp with time zone;

        ALTER TABLE "ImportDelivery"
        ALTER COLUMN "Revised_ETA_Port"
        TYPE timestamp with time zone
        USING "Revised_ETA_Port"::timestamp with time zone;
    """);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Revised_ETA_Port",
                table: "ImportDelivery",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Original_ETA_Port",
                table: "ImportDelivery",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
