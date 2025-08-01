using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeaFreightScheduleMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeaFreightScheduleMonitoring",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemCategory = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    Origin = table.Column<string>(type: "text", nullable: false),
                    BL = table.Column<string>(type: "text", nullable: false),
                    INV = table.Column<string>(type: "text", nullable: false),
                    Carrier_Forwarded = table.Column<string>(type: "text", nullable: false),
                    Port_Of_Discharge = table.Column<string>(type: "text", nullable: false),
                    Vessel_Name = table.Column<string>(type: "text", nullable: false),
                    Mode_Of_Shipment = table.Column<string>(type: "text", nullable: false),
                    Container_Size_No_Of_PKGS = table.Column<string>(type: "text", nullable: false),
                    Container_No = table.Column<string>(type: "text", nullable: false),
                    Trucker = table.Column<string>(type: "text", nullable: false),
                    Original_ETD = table.Column<DateOnly>(type: "date", nullable: false),
                    ATD = table.Column<DateOnly>(type: "date", nullable: false),
                    Original_ETA = table.Column<DateOnly>(type: "date", nullable: false),
                    Latest_ETA = table.Column<DateOnly>(type: "date", nullable: false),
                    ATA = table.Column<DateOnly>(type: "date", nullable: false),
                    ATB_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ATB_Time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Vessel_Remarks = table.Column<string>(type: "text", nullable: false),
                    Have_Job_Operation = table.Column<bool>(type: "boolean", nullable: false),
                    With_Special_Permit = table.Column<bool>(type: "boolean", nullable: false),
                    BERTH_Leadtime = table.Column<string>(type: "text", nullable: false),
                    ETA_BIPH = table.Column<DateOnly>(type: "date", nullable: false),
                    Orig_RDD = table.Column<DateOnly>(type: "date", nullable: false),
                    Requested_Del_Date_To_Trucker = table.Column<DateOnly>(type: "date", nullable: false),
                    Requested_Del_Time_To_Trucker = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Actual_Delivery = table.Column<DateOnly>(type: "date", nullable: false),
                    Actual_Del_Time_To_Trucker = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Based_On_BERTH_BIPH_Leadtime = table.Column<string>(type: "text", nullable: false),
                    Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend = table.Column<decimal>(type: "numeric", nullable: false),
                    Step_1 = table.Column<DateOnly>(type: "date", nullable: false),
                    Step_2 = table.Column<DateOnly>(type: "date", nullable: false),
                    Step_3 = table.Column<DateOnly>(type: "date", nullable: false),
                    Step_4 = table.Column<DateOnly>(type: "date", nullable: false),
                    Step_5 = table.Column<DateOnly>(type: "date", nullable: false),
                    Step_6 = table.Column<DateOnly>(type: "date", nullable: false),
                    Actual_Status = table.Column<DateOnly>(type: "date", nullable: false),
                    Shipment_Processing_Remarks = table.Column<DateOnly>(type: "date", nullable: false),
                    Bobtail_Date = table.Column<string>(type: "text", nullable: false),
                    Requested_Pick_Up_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Date_Return_of_Empty_Cntr = table.Column<DateOnly>(type: "date", nullable: true),
                    FreeTime_Valid_Until = table.Column<DateOnly>(type: "date", nullable: true),
                    No_of_Days_with_Detention_Estimate_Only = table.Column<decimal>(type: "numeric", nullable: false),
                    No_of_Days_of_Free_Time = table.Column<decimal>(type: "numeric", nullable: false),
                    Requested_Del_Date_To_Ship = table.Column<DateOnly>(type: "date", nullable: true),
                    Priority_Container = table.Column<string>(type: "text", nullable: false),
                    Earliest_Shortage_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Request_to_Unload_AM_or_PM = table.Column<string>(type: "text", nullable: false),
                    Random_Boolean = table.Column<bool>(type: "boolean", nullable: false),
                    Final_Remarks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaFreightScheduleMonitoring", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeaFreightScheduleMonitoring");
        }
    }
}
