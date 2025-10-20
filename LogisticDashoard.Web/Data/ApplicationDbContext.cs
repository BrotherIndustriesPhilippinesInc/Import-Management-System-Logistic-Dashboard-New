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

    }
}
