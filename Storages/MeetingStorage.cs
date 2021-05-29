using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Storages.Extensions;

namespace Storages
{
    public interface IMeetingStorage
    {
        Task<int> CreateMeeting(Meeting meeting);

        Task<Meeting[]> GetPossibleMeetings(DateTime date);

        Task<Meeting[]> GetMeetings(int offset, int limit, string query, DateTime date);

        Task<Meeting> UpdateMeeting(int id, UpdateMeetingDto updateMeetingDto);

        Task<bool> DeleteMeeting(int id);
    }
    
    public class MeetingStorage : IMeetingStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public MeetingStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> CreateMeeting(Meeting meeting)
        {
            await using var ctx = _contextFactory.Create();
            ctx.Meetings.Add(meeting);
            await ctx.SaveChangesAsync();
            return meeting.Id;
        }

        /// <summary>
        /// Получение встреч, которые можно провести в указанную дату
        /// </summary>
        public async Task<Meeting[]> GetPossibleMeetings(DateTime date)
        {
            await using var ctx = _contextFactory.Create();
            var meetings = await ctx.Meetings
                .Include(meeting => meeting.Client)
                .Where(
                    meeting => 
                        meeting.AvailableTimeStart.Date == date.Date &&
                        meeting.AvailableTimeEnd - TimeSpan.FromMinutes(meeting.DurationInMinutes) >= date)
                .OrderBy(meeting => meeting.Id)
                .ToArrayAsync();
            return meetings;
        }

        public async Task<Meeting[]> GetMeetings(int offset, int limit, string query, DateTime date)
        {
            await using var ctx = _contextFactory.Create();
            var meetings = await ctx.Meetings
                .Include(meeting => meeting.Client)
                .Where(meeting => meeting.AvailableTimeStart.Date == date.Date)
                .Search(query)
                .OrderBy(meeting => meeting.Id)
                .LimitByOffset(offset, limit)
                .ToArrayAsync();
            return meetings;
        }

        public async Task<Meeting> UpdateMeeting(int id, UpdateMeetingDto updateMeetingDto)
        {
            await using var ctx = _contextFactory.Create();
            var meetingToUpdate = await ctx.Meetings
                .Include(meeting => meeting.Client)
                .FirstOrDefaultAsync(meeting => meeting.Id == id);
            if (meetingToUpdate == null)
            {
                return null;
            }

            meetingToUpdate.AvailableTimeStart = updateMeetingDto.AvailableTimeStart;
            meetingToUpdate.AvailableTimeEnd = updateMeetingDto.AvailableTimeEnd;
            meetingToUpdate.DurationInMinutes = updateMeetingDto.DurationInMinutes;
            meetingToUpdate.Coordinate = updateMeetingDto.Coordinate;
            
            ctx.Meetings.Update(meetingToUpdate);
            await ctx.SaveChangesAsync();
            
            return meetingToUpdate;
        }

        public async Task<bool> DeleteMeeting(int id)
        {
            await using var ctx = _contextFactory.Create();
            var meeting = await ctx.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return false;
            }
            ctx.Meetings.Remove(meeting);
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}