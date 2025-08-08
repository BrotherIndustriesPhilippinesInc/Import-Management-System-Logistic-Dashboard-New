using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticDashboard.Web.Migrations
{
    /// <inheritdoc />
    public partial class AllStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "With_Special_Permit",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Transit_Days_ATD_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Step_6",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Step_5",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Step_4",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Step_3",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Step_2",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Step_1",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Requested_Pick_Up_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requested_Del_Time_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Requested_Del_Date_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Requested_Del_Date_To_Ship",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Random_Boolean",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Original_ETD",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Original_ETA",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Orig_RDD",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "No_of_Days_with_Detention_Estimate_Only",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "No_of_Days_of_Free_Time",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "No_Of_Days_Delayed_ETD_ATD",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "No_Of_Days_Delayed_ETA_ATB",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "No_Of_Days_Delayed_ETA_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Latest_ETA",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Have_Job_Operation",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FreeTime_Valid_Until",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Earliest_Shortage_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ETA_BIPH",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Date_Return_of_Empty_Cntr",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Actual_Delivery",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Actual_Del_Time_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "ATD",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "ATB_Time",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "ATB_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "With_Special_Permit",
                table: "SeaFreightScheduleMonitoring",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Transit_Days_ATD_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_6",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_5",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_4",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_3",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_2",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Step_1",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Requested_Pick_Up_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Requested_Del_Time_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Requested_Del_Date_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Requested_Del_Date_To_Ship",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Random_Boolean",
                table: "SeaFreightScheduleMonitoring",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Original_ETD",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Original_ETA",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Orig_RDD",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "No_of_Days_with_Detention_Estimate_Only",
                table: "SeaFreightScheduleMonitoring",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "No_of_Days_of_Free_Time",
                table: "SeaFreightScheduleMonitoring",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "No_Of_Days_Delayed_ETD_ATD",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "No_Of_Days_Delayed_ETA_ATB",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "No_Of_Days_Delayed_ETA_ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Latest_ETA",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "Have_Job_Operation",
                table: "SeaFreightScheduleMonitoring",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FreeTime_Valid_Until",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Earliest_Shortage_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ETA_BIPH",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date_Return_of_Empty_Cntr",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend",
                table: "SeaFreightScheduleMonitoring",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Actual_Delivery",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Actual_Del_Time_To_Trucker",
                table: "SeaFreightScheduleMonitoring",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ATD",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ATB_Time",
                table: "SeaFreightScheduleMonitoring",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ATB_Date",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ATA",
                table: "SeaFreightScheduleMonitoring",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
