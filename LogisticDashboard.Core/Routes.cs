using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class Routes
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ICollection<SailingSchedule>? SailingSchedule { get; set; }
    }
}
