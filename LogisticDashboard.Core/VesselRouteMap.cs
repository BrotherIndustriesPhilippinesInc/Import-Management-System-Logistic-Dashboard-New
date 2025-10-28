using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class VesselRouteMap
    {
        public int Id { get; set; }
        public string VesselName { get; set; }

        public string OriginPortName { get; set; }
        [Column(TypeName = "jsonb")]
        public Coordinates? OriginPortCoordinates { get; set; }

        public string DestinationPortName { get; set; }
        [Column(TypeName = "jsonb")]
        public Coordinates? DestinationPortCoordinates { get; set; }

        public int ShipId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
