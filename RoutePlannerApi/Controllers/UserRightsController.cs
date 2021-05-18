using System;
using System.Threading.Tasks;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/users")]
    [RightsAuthorize(Right.Admin)]
    [ApiController]
    public class UserRightsController : Controller
    {
        private readonly IUserRightStorage _userRightStorage;

        public UserRightsController(IUserRightStorage userRightStorage)
        {
            _userRightStorage = userRightStorage;
        }

        /// <summary>
        /// Выдача права пользователю
        /// </summary>
        [HttpPost("{id:int}/rights/{right:int}")]
        public async Task<ActionResult> AddRightToUser(int id, int right)
        {
            if (!Enum.IsDefined(typeof(Right), right))
            {
                return BadRequest($"Right {right} is invalid");
            }

            var createdRight = await _userRightStorage.AddRightToUser(id, (Right) right);
            return Ok(createdRight);
        }
    }
}