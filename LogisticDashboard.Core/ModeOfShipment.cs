using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ModeOfShipment
    {
        public int Id { get; set; }
        public string? FCLProcessFlow_Image_Link { get; set; }
        public string? LCLProcessFlow_Image_Link { get; set; }

        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
