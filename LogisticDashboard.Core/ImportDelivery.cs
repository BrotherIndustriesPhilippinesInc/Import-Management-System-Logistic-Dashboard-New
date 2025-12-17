using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ImportDelivery
    {
        public int Id { get; set; }
        public string BIL_No { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string? Reasons { get; set; }
        public string? BIPH_Action { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
