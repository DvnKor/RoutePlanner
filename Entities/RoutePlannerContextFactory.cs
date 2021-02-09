using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public interface IRoutePlannerContextFactory
    {
        RoutePlannerContext Create();
    }
    
    public class RoutePlannerContextFactory : IRoutePlannerContextFactory
    {
        private DbContextOptions<RoutePlannerContext> Options { get; }

        public RoutePlannerContextFactory()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PostgreSQLConnection"].ConnectionString;
            Options = new DbContextOptionsBuilder<RoutePlannerContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public RoutePlannerContext Create() => new RoutePlannerContext(Options);
    }
}