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
    }
}
