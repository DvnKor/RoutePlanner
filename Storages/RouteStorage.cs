using System;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IRouteStorage
    {
        Task<Route> GetCurrentRoute(int managerId);

        Task<int> AddRoute(Route route);

        Task<bool> DeleteRoute(int id);
    }

    public class RouteStorage : IRouteStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public RouteStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Route> GetCurrentRoute(int managerId)
        {
            await using var ctx = _contextFactory.Create();
            var suitableRoute = await ctx.Routes
                .Include(route => route.ManagerSchedule)
                .FirstOrDefaultAsync(route =>
                    route.ManagerSchedule.UserId == managerId &&
                    route.ManagerSchedule.StartTime.Date == DateTime.UtcNow.AddHours(5).Date);
            return suitableRoute;
        }

        public async Task<int> AddRoute(Route route)
        {
            await using var ctx = _contextFactory.Create();
            ctx.Routes.Add(route);
            await ctx.SaveChangesAsync();
            return route.Id;
        }

        public async Task<bool> DeleteRoute(int id)
        {
            await using var ctx = _contextFactory.Create();
            var entry = new Route {Id = id};
            ctx.Routes.Attach(entry);
            ctx.Routes.Remove(entry);
            var deleted = await ctx.SaveChangesAsync() > 0;
            return deleted;
        }
    }
}