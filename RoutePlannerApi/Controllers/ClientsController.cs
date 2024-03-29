using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientStorage _clientStorage;

        public ClientsController(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }
        
        /// <summary>
        /// Создание клиента
        /// </summary>
        [HttpPost("")]
        [RightsAuthorize(Right.Admin)]
        public async Task<ActionResult> CreateClient([FromBody] Client client)
        {
            var clientId = await _clientStorage.CreateClient(client);
            return Ok(client);
        }
        
        /// <summary>
        /// Обновление клиента
        /// </summary>
        [HttpPut("{id:int}")]
        [RightsAuthorize(Right.Admin)]
        public async Task<ActionResult> UpdateClient(int id, [FromBody] UpdateClientDto updateClientDto)
        {
            var updatedClient = await _clientStorage.UpdateClient(id, updateClientDto);
            if (updatedClient == null)
            {
                return NotFound(id);
            }

            return Ok(updatedClient);
        }
        
        /// <summary>
        /// Получение клиентов
        /// </summary>
        [HttpGet("")]
        [RightsAuthorize(Right.Admin)]
        public async Task<ActionResult> GetClients(
            [FromQuery] int offset, 
            [FromQuery] int limit,
            [FromQuery] string query)
        {
            var clients = await _clientStorage.GetClients(offset, limit, query);
            return Ok(clients);
        }
        
        /// <summary>
        /// Удаление клиента
        /// </summary>
        [HttpDelete("{id:int}")]
        [RightsAuthorize(Right.Admin)]
        public async Task<ActionResult> DeleteClient(int id)
        {
            var deleted = await _clientStorage.DeleteClient(id);
            return Ok(id);
        }
    }
}