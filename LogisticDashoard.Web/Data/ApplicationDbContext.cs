using LogisticDashboard.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace LogisticDashboard.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<SeaFreightScheduleMonitoring> SeaFreightScheduleMonitoring { get; set; }

        public DbSet<AirFreightScheduleMonitoring> AirFreightScheduleMonitoring { get; set; }
        public DbSet<LogisticDashboard.Core.SailingSchedule> SailingSchedule { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.PortUtilization> PortUtilization { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.BerthingStatus> BerthingStatus { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Incoterms> Incoterms { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.VesselRouteMap> VesselRouteMap { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.PHPortMap> PHPortMap { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Users> Users { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.DeliveryLeadtime> DeliveryLeadtime { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.DeliveryLeadtimeData> DeliveryLeadtimeData { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ModeOfShipment> ModeOfShipment { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ContainerVisualization> ContainerVisualization { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.CourierInformation> CourierInformation { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.DHL> DHL { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ShippingInstruction> ShippingInstruction { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.PhilippineHolidays> PhilippineHolidays { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportPICInformation> ImportPICInformation { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Flowchart> Flowchart { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.LogisticCost> LogisticCost { get; set; } = default!;

    }
}
