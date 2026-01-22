using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class AirFreightScheduleMonitoring
    {
        //SHIPMENT DETAILS
        [Key]
        public int Id { get; set; }
        public string ItemCategory { get; set; }
        public string Shipper { get; set; }
        //public string Origin { get; set; }
        public string AWB { get; set; }
        public string Forwarder_Courier { get; set; }
        public string Broker { get; set; }
        public string Flight_Detail { get; set; }
        public string Invoice_No { get; set; }
        public string Freight_Term { get; set; }
        public string No_Of_Pkgs { get; set; }

        //FLIGHT STATUS
        public string Original_ETD { get; set; }
        public string ATD { get; set; }
        public string Original_ETA { get; set; }
        public string Latest_ETA { get; set; }
        public string ATA { get; set; }
        public string Flight_Status_Remarks { get; set; }

        //Delivery
        public string Import_Permit_Status { get; set; }

        //SPECIAL REQUIREMENTS
        public string Have_Arrangement { get; set; }
        public string With_Special_Permit { get; set; }

        //Delivery
        public string ATA_Port_BIPH_Leadtime { get; set; }
        public string ETA_BIPH_Manual_Computation { get; set; }
        public string Requested_Del_Date_To_Ship { get; set; }
        public string Earliest_Shortage_Date { get; set; }
        public string Actual_Del { get; set; }
        public string Status { get; set; }
        public string Import_Remarks { get; set; }
        public string System_Update { get; set; }

        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
