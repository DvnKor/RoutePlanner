using System.Linq;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public static class UsersQueryExtensions
    {
        public static IQueryable<User> WithRights(this IQueryable<User> query)
        {
            return query
                .Include(user => user.UserRights)
                .ThenInclude(userRight => userRight.RightInfo);
        }
    }
}