using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Models;
using RoutePlannerApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly IMapper _mapper;

        public RoutesController(RoutesRepository routesRepository, IMapper mapper)
        {
            _routesRepository = routesRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех актуальных путей менеджеров
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<RouteDto> GetAllRoutes()
        {
            var allRoutes = _routesRepository.GetAllRoutes();
            var allRoutesDto = new List<RouteDto>();
            foreach (var (id, route) in allRoutes)
            {
                allRoutesDto.Add(new RouteDto(id,
                    route.Select(customer => _mapper.Map<Customer, CustomerDto>(customer)).ToList()));
            }

            return allRoutesDto;
        }

        /// <summary>
        /// Получение пути менеджера с id = managerId
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        [HttpGet("{managerId}")]
        public List<Customer> GetManagerRoute(int managerId)
        {
            return _routesRepository.GetManagerRoute(managerId);
        }
    }
}