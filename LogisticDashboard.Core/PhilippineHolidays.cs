using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class PhilippineHolidays
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Holiday { get; set; }
        public string Agency_Office { get; set; }
        public string Port { get; set; }
        public string Status { get; set; }

        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

    }
}
