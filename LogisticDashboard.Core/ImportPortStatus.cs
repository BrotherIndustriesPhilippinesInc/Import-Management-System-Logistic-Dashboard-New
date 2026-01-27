using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ImportPortStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PortUtilizationStatus { get; set; }
        public string BerthingStatus { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime LastUpdated { get; set; }
    }
}
