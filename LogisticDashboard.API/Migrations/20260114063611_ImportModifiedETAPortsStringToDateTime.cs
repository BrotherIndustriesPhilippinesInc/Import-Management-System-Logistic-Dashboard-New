using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.API.Migrations
{
    public partial class ImportModifiedETAPortsStringToDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔹 CLEAN BAD DATA FIRST
            migrationBuilder.Sql("""
                UPDATE "ImportDashboards"
                SET "Original_ETA_Port" = NULL
                WHERE "Original_ETA_Port" = '';

                UPDATE "ImportDashboards"
                SET "Revised_ETA_Port" = NULL
                WHERE "Revised_ETA_Port" = '';
            """);

            migrationBuilder.Sql("""
                UPDATE "ImportDeliveryDashboards"
                SET "Original_ETA_Port" = NULL
                WHERE "Original_ETA_Port" = '';

                UPDATE "ImportDeliveryDashboards"
                SET "Revised_ETA_Port" = NULL
                WHERE "Revised_ETA_Port" = '';
            """);

            migrationBuilder.Sql("""
                UPDATE "ImportPortUtilization"
                SET "Original_ETA_Port" = NULL
                WHERE "Original_ETA_Port" = '';

                UPDATE "ImportPortUtilization"
                SET "Revised_ETA_Port" = NULL
                WHERE "Revised_ETA_Port" = '';
            """);

            migrationBuilder.Sql("""
                UPDATE "ImportBerthingStatus"
                SET "Original_ETA_Port" = NULL
                WHERE "Original_ETA_Port" = '';

                UPDATE "ImportBerthingStatus"
                SET "Revised_ETA_Port" = NULL
                WHERE "Revised_ETA_Port" = '';
            """);

            // 🔹 CONVERT TYPES USING PostgreSQL CAST
            migrationBuilder.Sql("""
                ALTER TABLE "ImportDashboards"
                ALTER COLUMN "Original_ETA_Port"
                TYPE timestamptz
                USING "Original_ETA_Port"::timestamptz;

                ALTER TABLE "ImportDashboards"
                ALTER COLUMN "Revised_ETA_Port"
                TYPE timestamptz
                USING "Revised_ETA_Port"::timestamptz;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "ImportDeliveryDashboards"
                ALTER COLUMN "Original_ETA_Port"
                TYPE timestamptz
                USING "Original_ETA_Port"::timestamptz;

                ALTER TABLE "ImportDeliveryDashboards"
                ALTER COLUMN "Revised_ETA_Port"
                TYPE timestamptz
                USING "Revised_ETA_Port"::timestamptz;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "ImportPortUtilization"
                ALTER COLUMN "Original_ETA_Port"
                TYPE timestamptz
                USING "Original_ETA_Port"::timestamptz;

                ALTER TABLE "ImportPortUtilization"
                ALTER COLUMN "Revised_ETA_Port"
                TYPE timestamptz
                USING "Revised_ETA_Port"::timestamptz;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "ImportBerthingStatus"
                ALTER COLUMN "Original_ETA_Port"
                TYPE timestamptz
                USING "Original_ETA_Port"::timestamptz;

                ALTER TABLE "ImportBerthingStatus"
                ALTER COLUMN "Revised_ETA_Port"
                TYPE timestamptz
                USING "Revised_ETA_Port"::timestamptz;
            """);

            // 🔹 ACTION COLUMN NULLABLE (EF CAN HANDLE THIS)
            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ImportDashboards",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ImportDeliveryDashboards",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ImportPortUtilization",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ImportBerthingStatus",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "ImportDashboards"
                ALTER COLUMN "Original_ETA_Port" TYPE text,
                ALTER COLUMN "Revised_ETA_Port" TYPE text;

                ALTER TABLE "ImportDeliveryDashboards"
                ALTER COLUMN "Original_ETA_Port" TYPE text,
                ALTER COLUMN "Revised_ETA_Port" TYPE text;

                ALTER TABLE "ImportPortUtilization"
                ALTER COLUMN "Original_ETA_Port" TYPE text,
                ALTER COLUMN "Revised_ETA_Port" TYPE text;

                ALTER TABLE "ImportBerthingStatus"
                ALTER COLUMN "Original_ETA_Port" TYPE text,
                ALTER COLUMN "Revised_ETA_Port" TYPE text;
            """);
        }
    }
}
