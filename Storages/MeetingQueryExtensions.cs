using System.Linq;
using Entities.Models;

namespace Storages
{
    public static class MeetingQueryExtensions
    {
        public static IQueryable<Meeting> Search(this IQueryable<Meeting> meetings, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return meetings;
            }
            var lowerQuery = query.ToLower();
            return meetings.Where(meeting =>
                meeting.Name.ToLower().Contains(lowerQuery) ||
                (meeting.Client.Email != null && meeting.Client.Email.ToLower().Contains(lowerQuery)) ||
                (meeting.Client.MobilePhone != null && meeting.Client.MobilePhone.ToLower().Contains(lowerQuery)) ||
                (meeting.Client.Telegram != null && meeting.Client.Telegram.ToLower().Contains(lowerQuery)));
        }
    }
}