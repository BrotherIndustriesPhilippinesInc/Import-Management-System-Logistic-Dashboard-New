namespace LogisticDashboard.Web.DTO
{
    public class PortStatusUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PortUtilizationStatus { get; set; } // "Normal", "Critical", etc.
        public string BerthingStatus { get; set; }
    }
}
