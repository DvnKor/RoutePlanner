using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using RoutePlannerApi.Domain;
using RoutePlannerApi.Models;
using RoutePlannerApi.Repositories;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RightsAuthorize(Right.Manager)]
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
