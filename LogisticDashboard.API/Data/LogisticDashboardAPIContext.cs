using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.Core;

namespace LogisticDashboard.API.Data
{
    public class LogisticDashboardAPIContext : DbContext
    {
        public LogisticDashboardAPIContext (DbContextOptions<LogisticDashboardAPIContext> options)
            : base(options)
        {
        }

        public DbSet<LogisticDashboard.Core.SeaFreightScheduleMonitoring> SeaFreightScheduleMonitoring { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.AirFreightScheduleMonitoring> AirFreightScheduleMonitoring { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Routes> Routes { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.SailingSchedule> SailingSchedule { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.PortUtilization> PortUtilization { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Ports> Ports { get; set; } = default!;
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
        public DbSet<LogisticDashboard.Core.LogisticCostCourierAF> LogisticCostCourierAF { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportDelivery> ImportDelivery { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.Announcements> Announcements { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportDashboards> ImportDashboards { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportDeliveryDashboards> ImportDeliveryDashboards { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportPortUtilizationManila> ImportPortUtilizationManila { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportPortUtilizationBatangas> ImportPortUtilizationBatangas { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportBerthingStatusManila> ImportBerthingStatusManila { get; set; } = default!;
        public DbSet<LogisticDashboard.Core.ImportBerthingStatusBatangas> ImportBerthingStatusBatangas { get; set; } = default!;

    }
}
