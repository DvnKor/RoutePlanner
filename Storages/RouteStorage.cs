using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IRouteStorage
    {
        Task<Route> GetCurrentRoute(int managerId);

        Task<Dictionary<int, Route>> GetRoutesByScheduleIds(int[] managerScheduleIds);

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
                    route.ManagerSchedule.StartTime.Date == DateTime.UtcNow.AddHours(TimezoneProvider.OffsetInHours).Date);
            return suitableRoute;
        }
        
        public async Task<Dictionary<int, Route>> GetRoutesByScheduleIds(int[] managerScheduleIds)
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
                        .Where(meeting => meeting.EndTime < firstMeeting.StartTime)
                        .ToList();
                }
                currentRoute.SuitableMeetings = pastMeetings
                    .Concat(route.SuitableMeetings)
                    .ToList();
                
                var pastMeetingsDistance = pastMeetings.Sum(x => x.DistanceFromPrevious);
                currentRoute.Distance = pastMeetingsDistance + route.Distance;

                var pastMeetingsWaitingTime = pastMeetings.Sum(x => x.WaitingTime);
                currentRoute.WaitingTime = pastMeetingsWaitingTime + route.WaitingTime;
                
                currentRoute.FinishesAsPreferred = route.FinishesAsPreferred;
            }

            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteRoute(int id)
        {
            await using var ctx = _contextFactory.Create();
            var route = await ctx.Routes.FindAsync(id);
            if (route == null)
            {
                return false;
            }
            ctx.Routes.Remove(route);
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}