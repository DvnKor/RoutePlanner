using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RoutePlannerContext : DbContext
    {
        public RoutePlannerContext(DbContextOptions<RoutePlannerContext> options)
            : base(options)
        {
           // Database.EnsureDeleted();
           // Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            RightInfos.UpdateRange(RightInfoHelpers.DefaultRights);
        }

        public DbSet<Client> Clients { get; set; }
        
        public DbSet<Manager> Managers { get; set; }
        
        public DbSet<ManagerSchedule> ManagerSchedules { get; set; }
        
        public DbSet<ClientSchedule> ClientSchedules { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<RightInfo> RightInfos { get; set; }
        
        public DbSet<UserRight> UserRights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoutePlannerContext).Assembly);
        }
    }
}