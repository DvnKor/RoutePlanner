using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly ManagerRepository _managerRepository;

        public ManagersController(ManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        // GET: api/<ManagersController>
        [HttpGet]
        public IEnumerable<Manager> Get()
        {
            return _managerRepository.GetAllManagers();
        }



        // DELETE api/<ManagersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
