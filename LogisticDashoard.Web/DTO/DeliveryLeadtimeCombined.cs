using LogisticDashboard.Core;

namespace LogisticDashboard.Web.DTO
{
    public class DeliveryLeadtimeCombined
    {
        public IEnumerable<DeliveryLeadtime> DeliveryLeadtime { get; set; }
        public IEnumerable<DeliveryLeadtimeData> DeliveryLeadtimeData { get; set; }
    }
}
