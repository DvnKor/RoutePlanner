using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Storages
{
    public interface IUserStorage
    {
        Task<User> GetByEmail(string email);
        Task<int> AddUser(User user);
        Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto);
        Task<UserDto[]> GetUsers(int offset, int limit, string query, bool shouldHaveRights);
        Task DeleteUser(int id);
    }

    public class UserStorage : IUserStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public UserStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> GetByEmail(string email)
        {
            await using var ctx = _contextFactory.Create();
            return await ctx.Users
                .IncludeRights()
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<int> AddUser(User user)
        {
            await using var ctx = _contextFactory.Create();
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();
            return user.Id;
        }

        public async Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            await using var ctx = _contextFactory.Create();
            var userToUpdate = await ctx.Users
                .IncludeRights()
                .FirstOrDefaultAsync(user => user.Id == id);
            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.MobilePhone = updateUserDto.MobilePhone;
            userToUpdate.Telegram = updateUserDto.Telegram;
            userToUpdate.Coordinate = updateUserDto.Coordinate;
            ctx.Users.Update(userToUpdate);
            await ctx.SaveChangesAsync();
            
            return userToUpdate.ToDto();
        }

        public async Task<UserDto[]> GetUsers(
            int offset,
            int limit, 
            string query,
            bool shouldHaveRights)
        {
            await using var ctx = _contextFactory.Create();
            var usersQuery = ctx.Users.IncludeRights();
            if (shouldHaveRights)
            {
                usersQuery = usersQuery
                    .Where(user => user.UserRights != null && user.UserRights.Count > 0);
            }
            else
            {
                usersQuery = usersQuery
                    .Where(user => user.UserRights == null || user.UserRights.Count == 0);
            }
            var suitableUsers = await usersQuery
                .Search(query)
                .OrderBy(user => user.Id)
                .LimitByOffset(offset, limit)
                .Select(user => user.ToDto())
                .ToArrayAsync();
            return suitableUsers;
        }

        public async Task DeleteUser(int id)
        {
            await using var ctx = _contextFactory.Create();
            var entry = new User {Id = id};
            ctx.Users.Attach(entry);
            ctx.Users.Remove(entry);
            await ctx.SaveChangesAsync();
        }
    }
}