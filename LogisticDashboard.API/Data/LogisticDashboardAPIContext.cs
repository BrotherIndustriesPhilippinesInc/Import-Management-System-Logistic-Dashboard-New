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
    }
}
