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
        public async Task<ActionResult> GetCurrent()
        {
            var userDto = _userContext.User.ToDto();
            await SetPositionToUserDto(userDto);
            return Ok(userDto);
        }

        /// <summary>
        /// Обновление пользователя по id
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userStorage.UpdateUser(id, updateUserDto);
            var userDto = user?.ToDto();
            await SetPositionToUserDto(userDto);
            if (userDto == null)
            {
                return NotFound(id);
            }

            return Ok(userDto);
        }

        private async Task SetPositionToUserDto(UserDto userDto)
        {
            if (userDto?.Rights?.Length > 0)
            {
                var maxRight = userDto?.Rights?.Max();
                userDto.Position = await _rightInfoStorage.GetDescription(maxRight.Value);
            }
        }
    }
}