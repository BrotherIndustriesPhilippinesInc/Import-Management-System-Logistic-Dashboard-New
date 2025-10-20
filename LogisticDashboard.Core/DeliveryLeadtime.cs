using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class DeliveryLeadtime
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Carrier { get; set; }
        public string OriginPort { get; set; }
        public string DestinationPort { get; set; }
        public string VesselTransitLeadtime { get; set; }
        public string CustomsClearanceLeadtime { get; set; }
        public string TotalLeadtime { get; set; }
    }
}
