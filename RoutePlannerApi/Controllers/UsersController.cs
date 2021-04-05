using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IUserStorage _userStorage;
        private readonly IRightInfoStorage _rightInfoStorage;

        public UsersController(
            IUserContext userContext,
            IRightInfoStorage rightInfoStorage,
            IUserStorage userStorage)
        {
            _userContext = userContext;
            _rightInfoStorage = rightInfoStorage;
            _userStorage = userStorage;
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        [HttpGet("current")]
        public async Task<UserDto> GetCurrent()
        {
            var userDto = _userContext.User.ToDto();
            var maxRight = userDto.Rights.Max();
            userDto.Position = await _rightInfoStorage.GetDescription(maxRight);
            return userDto;
        }

        /// <summary>
        /// Обновление пользователя по id
        /// </summary>
        [HttpPut("{userId:int}")]
        public async Task<UserDto> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _userStorage.UpdateUser(userId, updateUserDto);
            return user?.ToDto();
        }
    }
}