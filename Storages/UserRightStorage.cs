using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Infrastructure.Rights;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IUserRightStorage
    {
        Task<UserRight> AddRightToUser(int userId, Right right);

        Task RemoveUserRight(int userId, Right right);
    }
    
    public class UserRightStorage : IUserRightStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public UserRightStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserRight> AddRightToUser(int userId, Right right)
        {
            await using var ctx = _contextFactory.Create();
            var userRight = await ctx.UserRights
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Right == right);
            if (userRight != null)
            {
                return userRight;
            }
            userRight = new UserRight {UserId = userId, Right = right};
            ctx.Add(userRight);
            await ctx.SaveChangesAsync();
            return userRight;
        }

        public async Task RemoveUserRight(int userId, Right right)
        {
            await using var ctx = _contextFactory.Create();
            var userRight = await ctx.UserRights
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Right == right);
            if (userRight == null)
            {
                return;
            }
            ctx.UserRights.Remove(userRight);
            await ctx.SaveChangesAsync();
        }
    }
}