using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class Incoterms
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Seller { get; set; }
        public string OriginTruckings { get; set; }
        public string OriginCustoms { get; set; }
        public string OriginTerminalCharges { get; set; }

        public string OceanFreightAirFreight { get; set; }

        public string DestinationTerminalCharges { get; set; }
        public string DestinationCustoms { get; set; }
        public string DestinationTrucking { get; set; }
        public string Buyer { get; set; }

    }
}
