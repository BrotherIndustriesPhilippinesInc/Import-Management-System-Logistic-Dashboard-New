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
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }
    }

    public class ImportDeliveryDashboards
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }
    }

    public class ImportPortUtilizationManila
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }

    }

    public class ImportPortUtilizationBatangas
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }

    }

    public class ImportBerthingStatusManila
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }

    }

    public class ImportBerthingStatusBatangas
    {
        public int Id { get; set; }
        public string BLNo { get; set; }
        public string Shipper { get; set; }
        public DateTime Original_ETA_Port { get; set; }
        public DateTime Revised_ETA_Port { get; set; }
        public string Reasons { get; set; }
        public string BIPH_Action { get; set; }
        public string Criteria { get; set; }
        public string? Action { get; set; }

    }

}
