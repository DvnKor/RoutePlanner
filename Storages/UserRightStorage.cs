using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Infrastructure.Rights;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IUserRightStorage
    {
        Task<UserRight> AddRightToUser(int id, Right right);
    }
    
    public class UserRightStorage : IUserRightStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public UserRightStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserRight> AddRightToUser(int id, Right right)
        {
            await using var ctx = _contextFactory.Create();
            var entity = await ctx.UserRights
                .FirstOrDefaultAsync(x => x.UserId == id && x.Right == right);
            if (entity != null)
            {
                var userRight = new UserRight {UserId = id, Right = right};
                ctx.Add(userRight);
                await ctx.SaveChangesAsync();
                return userRight;
            }
            return null;
        }
    }
}