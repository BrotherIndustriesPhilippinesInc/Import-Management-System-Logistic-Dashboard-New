using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ShippingInstruction
    {
        public int Id { get; set; }
        public string ShippingInstructionName { get; set; }
        public string? FileLinkEnglish { get; set; }
        public string? FileLinkJapanese { get; set; }

        public string Type { get; set; }

        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

    }
}
