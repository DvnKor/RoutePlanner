using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private RoutesRepository _routesRepository;

        public RoutesController(RoutesRepository routesRepository)
        {
            _routesRepository = routesRepository;
        }

        // GET: api/<RoutesController>
        [HttpGet]
        public Dictionary<int, List<Customer>> GetAllRoutes()
        {
            return _routesRepository.GetAllRoutes();
        }

        // GET api/<RoutesController>/5
        [HttpGet("{managerId}")]
        public List<Customer> GetManagerRoute(int managerId)
        {
            return _routesRepository.GetManagerRoute(managerId);
        }
    }
}