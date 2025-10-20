using LogisticDashboard.Core;

namespace LogisticDashboard.API.DTO
{
    public class PortsNoUtilizationDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required int Year { get; set; }

        public int Normal_Range_Overall_Yard_Utilization { get; set; }
        public int Normal_Range_Vessels_At_Berth { get; set; }
        public int Normal_Range_Vessels_At_Anchorage { get; set; }
    }
}
