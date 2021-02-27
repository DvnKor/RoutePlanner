using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RoutePlannerContext : DbContext
    {
        public RoutePlannerContext(DbContextOptions<RoutePlannerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Client> Clients { get; set; }
        
        public DbSet<Manager> Managers { get; set; }
        
        public DbSet<Organization> Organizations { get; set; }
        
        public DbSet<ManagerSchedule> ManagerSchedules { get; set; }
        
        public DbSet<ClientSchedule> ClientSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoutePlannerContext).Assembly);
        }
    }
}