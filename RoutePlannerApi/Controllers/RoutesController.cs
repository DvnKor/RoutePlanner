using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : Controller
    {
        private readonly IRouteStorage _routeStorage;

        public RoutesController(IRouteStorage routeStorage)
        {
            _routeStorage = routeStorage;
        }
        
        /// <summary>
        /// Получение текущего маршрута для менеджера
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult> GetCurrentRoute([FromQuery] int managerId)
        {
            var currentRoute = await _routeStorage.GetCurrentRoute(managerId);
            return Ok(currentRoute);
        }
    }
}