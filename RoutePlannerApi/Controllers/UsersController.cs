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
        private readonly IRightInfoStorage _rightInfoStorage;

        public UsersController(IUserContext userContext, IRightInfoStorage rightInfoStorage)
        {
            _userContext = userContext;
            _rightInfoStorage = rightInfoStorage;
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public async Task<UserDto> GetCurrent()
        {
            var userDto = _userContext.User.ToDto();
            var maxRight = userDto.Rights.Max();
            userDto.Position = await _rightInfoStorage.GetDescription(maxRight);
            return userDto;
        }
    }
}