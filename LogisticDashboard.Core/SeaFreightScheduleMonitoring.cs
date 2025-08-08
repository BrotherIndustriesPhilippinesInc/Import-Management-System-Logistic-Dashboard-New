using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class SeaFreightScheduleMonitoring
    {
        //SHIPMENT DETAILS
        [Key]
        public int Id { get; set; }
        public string ItemCategory { get; set; }
        public string Shipper { get; set; }
        public string Origin { get; set; }
        public string BL { get; set; }
        public string INV { get; set; }
        public string Carrier_Forwarded { get; set; }
        public string Port_Of_Discharge { get; set; }
        public string Vessel_Name { get; set; }
        public string Mode_Of_Shipment { get; set; }
        public string Container_Size_No_Of_PKGS { get; set; }
        public string Container_No { get; set; }
        public string Trucker { get; set; }

        //VESSEL STATUS
        public string Original_ETD { get; set; }
        public string ATD { get; set; }
        public string Original_ETA { get; set; }
        public string Latest_ETA { get; set; }
        public string ATA { get; set; }
        public string ATB_Date { get; set; }
        public string ATB_Time { get; set; }
        public string No_Of_Days_Delayed_ETD_ATD { get; set; }
        public string No_Of_Days_Delayed_ETA_ATA { get; set; }
        public string No_Of_Days_Delayed_ETA_ATB { get; set; }
        public string Transit_Days_ATD_ATA { get; set; }
        public string Vessel_Status { get; set; }
        public string Vessel_Remarks { get; set; }

        //SPECIAL REQUIREMENTS
        public string Have_Job_Operation { get; set; }
        public string With_Special_Permit { get; set; }

        //DELIVERY
        public string BERTH_Leadtime { get; set; }
        public string ETA_BIPH { get; set; }
        public string Orig_RDD { get; set; }
        public string Requested_Del_Date_To_Trucker { get; set; }
        public string Requested_Del_Time_To_Trucker { get; set; }
        public string Actual_Delivery { get; set; }
        public string Actual_Del_Time_To_Trucker { get; set; }
        public string Based_On_BERTH_BIPH_Leadtime { get; set; }
        public string Actual_Leadtime_ATA_Port_ATA_BIPH_exclude_weekend { get; set; }

        //SHIPMENT PROCESSING STATUS
        public string Step_1 { get; set; }
        public string Step_2 { get; set; }
        public string Step_3 { get; set; }
        public string Step_4 { get; set; }
        public string Step_5 { get; set; }
        public string Step_6 { get; set; }
        public string Actual_Status { get; set; }
        public string Shipment_Processing_Remarks { get; set; }

        //BOBTAIL / DETENTION
        public string Bobtail_Date { get; set; }
        public string? Requested_Pick_Up_Date { get; set; }
        public string? Date_Return_of_Empty_Cntr { get; set; }
        public string? FreeTime_Valid_Until { get; set; }
        public string No_of_Days_with_Detention_Estimate_Only { get; set; }
        public string No_of_Days_of_Free_Time { get; set; }

        // MP / PURCHASING
        public string? Requested_Del_Date_To_Ship { get; set; }
        public string Priority_Container { get; set; }
        public string? Earliest_Shortage_Date { get; set; }
        public string Request_to_Unload_AM_or_PM { get; set; }

        public string Random_Boolean { get; set; }
        public string Final_Remarks { get; set; }

    }
}