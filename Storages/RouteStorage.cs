using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IRouteStorage
    {
        Task<Route> GetCurrentRoute(int managerId);

        Task<Dictionary<int, Route>> GetRoutes(int[] managerScheduleIds);

        Task<int> AddRoute(Route route);

        Task AddOrUpdateRoute(Route route);

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
        
        public async Task<Dictionary<int, Route>> GetRoutes(int[] managerScheduleIds)
        {
            await using var ctx = _contextFactory.Create();
            var routesDictionary = await ctx.Routes
                .Where(route => managerScheduleIds.Contains(route.ManagerScheduleId))
                .ToDictionaryAsync(
                    route => route.ManagerScheduleId,
                    route => route);
            return routesDictionary;
        }

        public async Task<int> AddRoute(Route route)
        {
            await using var ctx = _contextFactory.Create();
            ctx.Routes.Add(route);
            await ctx.SaveChangesAsync();
            return route.Id;
        }

        public async Task AddOrUpdateRoute(Route route)
        {
            await using var ctx = _contextFactory.Create();
            var currentRoute = await ctx.Routes
                .FirstOrDefaultAsync(r => r.ManagerScheduleId == route.ManagerScheduleId);
            if (currentRoute == null)
            {
                ctx.Routes.Add(route);
            }
            else
            {
                var firstMeeting = route.SuitableMeetings.FirstOrDefault();
                var pastMeetings = currentRoute.SuitableMeetings;
                if (firstMeeting != null)
                {
                    pastMeetings = pastMeetings
                        .Where(meeting => meeting.EndTime < firstMeeting?.StartTime)
                        .ToList();

                }
                currentRoute.SuitableMeetings = pastMeetings
                    .Concat(route.SuitableMeetings)
                    .ToList();
                
                //toDo рассчитать новое расстояние и время ожидания
                //existedRoute.Distance = route.Distance;
                //existedRoute.WaitingTime = route.WaitingTime;
                
                currentRoute.FinishesAsPreferred = route.FinishesAsPreferred;
            }

            await ctx.SaveChangesAsync();
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