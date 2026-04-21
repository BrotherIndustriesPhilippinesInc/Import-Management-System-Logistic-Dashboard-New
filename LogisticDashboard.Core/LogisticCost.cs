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
        public decimal TotalPHP { get; set; } //rename from TotalUSD to TotalPHP since TotalUSD will change base on the input exchange rate
        public string Origin { get; set; }

        //additional columns
        //public decimal Freight { get; set; }
        //public decimal Local { get; set; }
        //public decimal GoGreen { get; set; }
        //public decimal DS { get; set; }
        //public decimal FS { get; set; }
    }
}
