using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class CourierInformation
    {
        public int Id { get; set; }
        public string CourierName { get; set; }
        public string CourierImage { get; set; }
        public string BIPH_Account_No { get; set; }

        public DateTime LastUpdated { get; set; }
        public string LastUpdateBy { get; set; }
    }

    public class DHL
    {
        public int Id { get; set; }
        public string ImageLink { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdateBy { get; set; }
    }
}
