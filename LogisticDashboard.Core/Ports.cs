using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class Ports
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required int Year { get; set; }

        public int Normal_Range_Overall_Yard_Utilization { get; set; }
        public int Normal_Range_Vessels_At_Berth { get; set; }
        public int Normal_Range_Vessels_At_Anchorage { get; set; }

        public ICollection<PortUtilization>? PortUtilizations { get; set; }
    }
}
