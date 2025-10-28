using LogisticDashboard.Core;

namespace LogisticDashboard.Web.DTO
{
    public class CourierInformationDHLCombined
    {
        public IEnumerable<CourierInformation> DeliveryInformation { get; set; }
        public DHL Dhl { get; set; }
    }
}
