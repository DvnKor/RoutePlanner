using Contracts;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserContext _userContext;

        public UsersController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public UserDto GetCurrent()
        {
            return _userContext.User.ToDto();
        }
    }
}