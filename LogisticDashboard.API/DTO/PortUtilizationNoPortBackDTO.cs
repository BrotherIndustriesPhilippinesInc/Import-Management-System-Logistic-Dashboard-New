using LogisticDashboard.Core;

namespace LogisticDashboard.API.DTO
{
    public class PortUtilizationNoPortBackDTO
    {
        public int Id { get; set; }
        public required int PortId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Week { get; set; }
        public int Overall_Yard_Utilization { get; set; }

        public int Vessels_At_Berth { get; set; }
        public int Vessels_At_Anchorage_Waiting { get; set; }

        public DateTime Last_Update { get; set; }
    }
}
