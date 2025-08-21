using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticDashboard.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirFreightScheduleMonitoring",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemCategory = table.Column<string>(type: "text", nullable: false),
                    Shipper = table.Column<string>(type: "text", nullable: false),
                    AWB = table.Column<string>(type: "text", nullable: false),
                    Forwarder_Courier = table.Column<string>(type: "text", nullable: false),
                    Broker = table.Column<string>(type: "text", nullable: false),
                    Flight_Detail = table.Column<string>(type: "text", nullable: false),
                    Invoice_No = table.Column<string>(type: "text", nullable: false),
                    Freight_Term = table.Column<string>(type: "text", nullable: false),
                    No_Of_Pkgs = table.Column<string>(type: "text", nullable: false),
                    Original_ETD = table.Column<string>(type: "text", nullable: false),
                    ATD = table.Column<string>(type: "text", nullable: false),
                    Original_ETA = table.Column<string>(type: "text", nullable: false),
                    Latest_ETA = table.Column<string>(type: "text", nullable: false),
                    ATA = table.Column<string>(type: "text", nullable: false),
                    Flight_Status_Remarks = table.Column<string>(type: "text", nullable: false),
                    Import_Permit_Status = table.Column<string>(type: "text", nullable: false),
                    Have_Arrangement = table.Column<string>(type: "text", nullable: false),
                    With_Special_Permit = table.Column<string>(type: "text", nullable: false),
                    ATA_Port_BIPH_Leadtime = table.Column<string>(type: "text", nullable: false),
                    ETA_BIPH_Manual_Computation = table.Column<string>(type: "text", nullable: false),
                    Requested_Del_Date_To_Ship = table.Column<string>(type: "text", nullable: false),
                    Earliest_Shortage_Date = table.Column<string>(type: "text", nullable: false),
                    Actual_Del = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Import_Remarks = table.Column<string>(type: "text", nullable: false),
                    System_Update = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirFreightScheduleMonitoring", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

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
                    Original_ETD = table.Column<string>(type: "text", nullable: false),
                    ATD = table.Column<string>(type: "text", nullable: false),
                    Original_ETA = table.Column<string>(type: "text", nullable: false),
                    Latest_ETA = table.Column<string>(type: "text", nullable: false),
                    ATA = table.Column<string>(type: "text", nullable: false),
                    ATB_Date = table.Column<string>(type: "text", nullable: false),
                    ATB_Time = table.Column<string>(type: "text", nullable: false),
                    No_Of_Days_Delayed_ETD_ATD = table.Column<string>(type: "text", nullable: false),
                    No_Of_Days_Delayed_ETA_ATA = table.Column<string>(type: "text", nullable: false),
                    No_Of_Days_Delayed_ETA_ATB = table.Column<string>(type: "text", nullable: false),
                    Transit_Days_ATD_ATA = table.Column<string>(type: "text", nullable: false),
                    Vessel_Status = table.Column<string>(type: "text", nullable: false),
                    Vessel_Remarks = table.Column<string>(type: "text", nullable: false),
                    Have_Job_Operation = table.Column<string>(type: "text", nullable: false),
                    With_Special_Permit = table.Column<string>(type: "text", nullable: false),
                    BERTH_Leadtime = table.Column<string>(type: "text", nullable: false),
                    ETA_BIPH = table.Column<string>(type: "text", nullable: false),
                    Orig_RDD = table.Column<string>(type: "text", nullable: false),
                    Requested_Del_Date_To_Trucker = table.Column<string>(type: "text", nullable: false),
                    Requested_Del_Time_To_Trucker = table.Column<string>(type: "text", nullable: false),
                    Actual_Delivery = table.Column<string>(type: "text", nullable: false),
                    Actual_Del_Time_To_Trucker = table.Column<string>(type: "text", nullable: false),
                    Based_On_BERTH_BIPH_Leadtime = table.Column<string>(type: "text", nullable: false),
                    Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend = table.Column<string>(type: "text", nullable: false),
                    Step_1 = table.Column<string>(type: "text", nullable: false),
                    Step_2 = table.Column<string>(type: "text", nullable: false),
                    Step_3 = table.Column<string>(type: "text", nullable: false),
                    Step_4 = table.Column<string>(type: "text", nullable: false),
                    Step_5 = table.Column<string>(type: "text", nullable: false),
                    Step_6 = table.Column<string>(type: "text", nullable: false),
                    Actual_Status = table.Column<string>(type: "text", nullable: false),
                    Shipment_Processing_Remarks = table.Column<string>(type: "text", nullable: false),
                    Bobtail_Date = table.Column<string>(type: "text", nullable: false),
                    Requested_Pick_Up_Date = table.Column<string>(type: "text", nullable: true),
                    Date_Return_of_Empty_Cntr = table.Column<string>(type: "text", nullable: true),
                    FreeTime_Valid_Until = table.Column<string>(type: "text", nullable: true),
                    No_of_Days_with_Detention_Estimate_Only = table.Column<string>(type: "text", nullable: false),
                    No_of_Days_of_Free_Time = table.Column<string>(type: "text", nullable: false),
                    Requested_Del_Date_To_Ship = table.Column<string>(type: "text", nullable: true),
                    Priority_Container = table.Column<string>(type: "text", nullable: false),
                    Earliest_Shortage_Date = table.Column<string>(type: "text", nullable: true),
                    Request_to_Unload_AM_or_PM = table.Column<string>(type: "text", nullable: false),
                    Random_Boolean = table.Column<string>(type: "text", nullable: false),
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
                name: "AirFreightScheduleMonitoring");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SeaFreightScheduleMonitoring");
        }
    }
}
