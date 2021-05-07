using System.Threading.Tasks;
using Contracts;
using Infrastructure.Rights;
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

        public UsersController(
            IUserContext userContext,
            IUserStorage userStorage)
        {
            _userContext = userContext;
            _userStorage = userStorage;
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        [HttpGet("current")]
        public async Task<ActionResult> GetCurrent()
        {
            var userDto = _userContext.User.ToDto();
            return Ok(userDto);
        }

        /// <summary>
        /// Обновление пользователя по id
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            // Пользователь может обновлять только себя, если он не админ
            var currentUser = _userContext.User;
            if (!currentUser.HasRight(Right.Admin) && currentUser.Id != id)
            {
                return Forbid();
            }
            
            var user = await _userStorage.UpdateUser(id, updateUserDto);
            var userDto = user?.ToDto();
            if (userDto == null)
            {
                return NotFound(id);
            }

            return Ok(userDto);
        }
        
        /// <summary>
        /// Получение пользователей, которые не имеют ни одного права
        /// </summary>
        [RightsAuthorize(Right.Admin)]
        [HttpGet("without-rights")]
        public async Task<ActionResult> GetUsersWithoutRights()
        {
            var usersWithoutRights = await _userStorage.GetUsersWithoutRights();
            return Ok(usersWithoutRights);
        }

        /// <summary>
        /// Получение пользователей, которые имеют хотя бы одно право
        /// </summary>
        [RightsAuthorize(Right.Admin)]
        [HttpGet("with-rights")]
        public async Task<ActionResult> GetUsersWithAnyRight()
        {
            var usersWithAnyRight = await _userStorage.GetUsersWithAnyRight();
            return Ok(usersWithAnyRight);
        }
        
        /// <summary>
        /// Удаление пользователя по id
        /// </summary>
        [RightsAuthorize(Right.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userStorage.DeleteUser(id);
            return Ok();
        }
    }
}