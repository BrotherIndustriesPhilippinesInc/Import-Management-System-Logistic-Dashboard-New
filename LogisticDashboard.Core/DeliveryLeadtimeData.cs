using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class DeliveryLeadtimeData
    {
        public int Id { get; set; }
        public int? FY { get; set; }
        public string LeadtimeName { get; set; }
        public string Month { get; set; }
        public decimal? Actual_Ave { get; set; }
        public decimal? Target_Max { get; set; }
        public decimal? Target_Min { get; set; }
        public decimal? No_Of_BL { get; set; }

        public decimal? ActualFYAverage { get; set; }
        public decimal? TargetFYMax { get; set; }
        public decimal? TargetFYMin { get; set; }
        public decimal? NoOfBLFY { get; set; }
    }
}
