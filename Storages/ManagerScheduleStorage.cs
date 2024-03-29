using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Storages.Extensions;

namespace Storages
{
    public interface IManagerScheduleStorage
    {
        Task<ManagerSchedule[]> GetManagerSchedules(DateTime date);
        
        Task<ManagerSchedule[]> GetManagerScheduleForWeek(int managerId, DateTime weekDate);
        
        Task<int> CreateManagerSchedule(ManagerSchedule managerSchedule);

        Task<ManagerSchedule> UpdateManagerSchedule(
            int id, UpdateManagerScheduleDto updateManagerScheduleDto);

        Task<bool> DeleteManagerSchedule(int id);
    }

    public class ManagerScheduleStorage : IManagerScheduleStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public ManagerScheduleStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        
        public async Task<ManagerSchedule[]> GetManagerSchedules(DateTime date)
        {
            await using var ctx = _contextFactory.Create();
            var managerSchedules = await ctx.ManagerSchedules
                .Where(managerSchedule => managerSchedule.StartTime.AddHours(TimezoneProvider.OffsetInHours).Date == date.AddHours(TimezoneProvider.OffsetInHours).Date)
                .OrderBy(managerSchedule => managerSchedule.StartTime)
                .ToArrayAsync();
            return managerSchedules;
        }

        public async Task<ManagerSchedule[]> GetManagerScheduleForWeek(int managerId, DateTime weekDate)
        {
            await using var ctx = _contextFactory.Create();
            var startOfWeek = weekDate.StartOfWeek();
            var startOfNextWeek = weekDate.StartOfNextWeek();
            var managerScheduleForWeek = await ctx.ManagerSchedules
                .Where(managerSchedule => 
                    managerSchedule.UserId == managerId &&
                    managerSchedule.StartTime >= startOfWeek &&
                    managerSchedule.StartTime <= startOfNextWeek)
                .OrderBy(managerSchedule => managerSchedule.StartTime)
                .ToArrayAsync();
            return managerScheduleForWeek;
        }

        public async Task<int> CreateManagerSchedule(ManagerSchedule managerSchedule)
        {
            await using var ctx = _contextFactory.Create();
            ctx.ManagerSchedules.Add(managerSchedule);
            await ctx.SaveChangesAsync();
            return managerSchedule.Id;
        }

        public async Task<ManagerSchedule> UpdateManagerSchedule(
            int id, UpdateManagerScheduleDto updateManagerScheduleDto)
        {
            await using var ctx = _contextFactory.Create();
            var managerScheduleToUpdate = await ctx.ManagerSchedules
                .FirstOrDefaultAsync(managerSchedule => managerSchedule.Id == id);
            if (managerScheduleToUpdate == null)
            {
                return null;
            }

            managerScheduleToUpdate.StartTime = updateManagerScheduleDto.StartTime;
            managerScheduleToUpdate.EndTime = updateManagerScheduleDto.EndTime;
            managerScheduleToUpdate.StartCoordinate = updateManagerScheduleDto.StartCoordinate;
            managerScheduleToUpdate.EndCoordinate = updateManagerScheduleDto.EndCoordinate;
            
            ctx.ManagerSchedules.Update(managerScheduleToUpdate);
            await ctx.SaveChangesAsync();
            
            return managerScheduleToUpdate;
        }

        public async Task<bool> DeleteManagerSchedule(int id)
        {
            await using var ctx = _contextFactory.Create();
            var managerSchedule = await ctx.ManagerSchedules.FindAsync(id);
            if (managerSchedule == null)
            {
                return false;
            }
            ctx.ManagerSchedules.Remove(managerSchedule);
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}