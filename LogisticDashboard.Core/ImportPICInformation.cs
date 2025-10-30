using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class ImportPICInformation
    {
        public int Id { get; set; }
        public string ModeOfShipment { get; set; }
        public string Supplier { get; set; }
        public string ShippingMainPICStaff { get; set; }
        public string ShippingSubPICStaff { get; set; }
        public string Supervisor { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class Flowchart
    {
        public string Id { get; set; }
        [Column(TypeName = "jsonb")]
        public string DrawflowData { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
