using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationStorage _organizationStorage;

        public OrganizationController(IOrganizationStorage organizationStorage)
        {
            _organizationStorage = organizationStorage;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOrganization(string name)
        {
            return Ok(await _organizationStorage.CreateOrganizationAsync(name));
        }
    }
}