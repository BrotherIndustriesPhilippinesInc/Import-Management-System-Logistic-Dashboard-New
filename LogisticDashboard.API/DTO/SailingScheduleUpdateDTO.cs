namespace LogisticDashboard.API.DTO
{
    public class SailingScheduleUpdateDTO
    {
        public int Id { get; set; }

        public string Week { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string? VesselName { get; set; }

        public string? VoyNo { get; set; }

        public string? Origin { get; set; }

        public string? OriginalETD { get; set; }

        public string? OriginalETAMNL { get; set; }

        public string? LatestETD { get; set; }

        public string? LatestETAMNL { get; set; }

        public string? TransitDays { get; set; }

        public int? DelayDeparture { get; set; }

        public int? DelayArrival { get; set; }

        public string? Remarks { get; set; }
    }
}
