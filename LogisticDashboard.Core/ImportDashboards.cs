using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ImportDashboards
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public string Original_ETA_Port { get; set; }
        public string Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string Action { get; set; }
    }

    public class ImportDeliveryDashboards
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public string Original_ETA_Port { get; set; }
        public string Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string Action { get; set; }
    }

    public class ImportPortUtilization
    {
        public int Id { get; set; }
        public int PortId { get; set; }
        //relationship
        public Ports Port { get; set; }

        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public string Original_ETA_Port { get; set; }
        public string Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }

        public string Action { get; set; }

    }

    public class ImportBerthingStatus
    {
        public int Id { get; set; }
        public int PortId { get; set; }
        //relationship
        public Ports Port { get; set; }

        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public string Original_ETA_Port { get; set; }
        public string Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }

        public string Action { get; set; }

    }
}
