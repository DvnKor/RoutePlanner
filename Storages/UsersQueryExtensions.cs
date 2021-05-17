using System.Linq;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public static class UsersQueryExtensions
    {
        public static IQueryable<User> WithRights(this IQueryable<User> users)
        {
            return users
                .Include(user => user.UserRights)
                .ThenInclude(userRight => userRight.RightInfo);
        }

        public static IQueryable<User> Search(this IQueryable<User> users, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return users;
            }
            var lowerQuery = query.ToLower();
            return users.Where(user =>
                user.Name.ToLower().Contains(lowerQuery) ||
                (user.Email != null && user.Email.ToLower().Contains(lowerQuery)) ||
                (user.MobilePhone != null && user.MobilePhone.ToLower().Contains(lowerQuery)) ||
                (user.Telegram != null && user.Telegram.ToLower().Contains(lowerQuery)));
        }
    }
}