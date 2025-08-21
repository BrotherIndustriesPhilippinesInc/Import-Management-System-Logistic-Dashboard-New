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
    }
}
