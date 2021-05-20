using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerOld.Domain;
using RoutePlannerOld.Models;
using RoutePlannerOld.Repositories;

namespace RoutePlannerOld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : Controller
    {
        private readonly ManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public ManagersController(ManagerRepository managerRepository, IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех менеджеров
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ManagerDto> Get()
        {
            var managers = _managerRepository.GetAllManagers();
            return managers.Select(manager => _mapper.Map<Manager, ManagerDto>(manager));
        }
    }
}
