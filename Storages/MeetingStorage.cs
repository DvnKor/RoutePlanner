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

        Task<Meeting[]> GetMeetings(DateTime date);

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

        public async Task<Meeting[]> GetMeetings(DateTime date)
        {
            await using var ctx = _contextFactory.Create();
            var meetings = await ctx.Meetings
                .Include(meeting => meeting.Client)
                .Where(meeting => meeting.StartTime.Date == date.Date && meeting.StartTime > date)
                .OrderBy(meeting => meeting.Id)
                .ToArrayAsync();
            return meetings;
        }

        public async Task<Meeting[]> GetMeetings(int offset, int limit, string query, DateTime date)
        {
            await using var ctx = _contextFactory.Create();
            var meetings = await ctx.Meetings
                .Include(meeting => meeting.Client)
                .Where(meeting => meeting.StartTime.Date == date.Date)
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

            meetingToUpdate.StartTime = updateMeetingDto.StartTime;
            meetingToUpdate.EndTime = updateMeetingDto.EndTime;
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