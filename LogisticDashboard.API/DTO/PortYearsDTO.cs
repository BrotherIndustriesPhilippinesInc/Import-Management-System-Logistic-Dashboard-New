namespace LogisticDashboard.API.DTO
{
    public class PortsYearsDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<int> Years { get; set; } = new();
    }
}
