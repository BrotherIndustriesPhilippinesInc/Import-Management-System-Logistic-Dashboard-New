using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class BerthingStatus
    {
        public int Id { get; set; }
        public string PortName { get; set; }

        public string? Normal_Range_VesselsAtBerth { get; set; }
        public string? VesselsAtBerth { get; set; }

        public string? Normal_Range_VesselsAtAnchorage { get; set; }
        public string? VesselsAtAnchorage { get; set; }

        public int Year { get; set; }

        public DateTime Date { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
