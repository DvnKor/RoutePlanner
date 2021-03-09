using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoutePlannerApi.Controllers
{
    public class User
    {
        public string Name { get; set; }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : Controller
    {
        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public User GetCurrent()
        {
            return new User {Name = "kek"};
        }
    }
}