using System.Threading.Tasks;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [RightsAuthorize(Right.Manager)]
    [ApiController]
    public class RoutesController : Controller
    {
        private readonly IRouteStorage _routeStorage;
        private readonly IUserContext _userContext;

        public RoutesController(IRouteStorage routeStorage, IUserContext userContext)
        {
            _routeStorage = routeStorage;
            _userContext = userContext;
        }
        
        /// <summary>
        /// Получение текущего маршрута для менеджера
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult> GetCurrentRoute([FromQuery] int managerId)
        {
            var currentUser = _userContext.User;
            if (!currentUser.HasRight(Right.Admin) && currentUser.Id != managerId)
            {
                return Forbid();
            }
            
            var currentRoute = await _routeStorage.GetCurrentRoute(managerId);
            return Ok(currentRoute);
        }
    }
}