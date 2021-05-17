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
                user.Name.Contains(lowerQuery) ||
                user.Email.Contains(lowerQuery) ||
                user.MobilePhone.Contains(lowerQuery) ||
                user.Telegram.Contains(lowerQuery));
        }
    }
}