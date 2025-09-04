namespace LogisticDashboard.API.DTO
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public List<SailingScheduleNoRoutesDTO> Schedules { get; set; }
    }

}
