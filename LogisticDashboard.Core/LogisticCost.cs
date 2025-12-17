using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class LogisticCost
    {
        public int Id { get; set; }
        public string KGS { get; set; }
        public decimal TotalUSD { get; set; }
        public string Origin { get; set; }
    }
}
