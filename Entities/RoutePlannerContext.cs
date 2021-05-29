using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RoutePlannerContext : DbContext
    {
        public RoutePlannerContext(DbContextOptions<RoutePlannerContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        
        public DbSet<ManagerSchedule> ManagerSchedules { get; set; }
        
        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<RightInfo> RightInfos { get; set; }
        
        public DbSet<UserRight> UserRights { get; set; }
        
        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoutePlannerContext).Assembly);
        }
    }
}