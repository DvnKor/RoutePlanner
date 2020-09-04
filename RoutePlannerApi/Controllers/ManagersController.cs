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
    public class ManagersController : ControllerBase
    {
        private readonly ManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public ManagersController(ManagerRepository managerRepository, IMapper mapper)
        {
            _managerRepository = managerRepository;
            _mapper = mapper;
        }

        // GET: api/<ManagersController>
        [HttpGet]
        public IEnumerable<ManagerDto> Get()
        {
            var managers = _managerRepository.GetAllManagers();
            return managers.Select(manager => _mapper.Map<Manager, ManagerDto>(manager));
        }



        // DELETE api/<ManagersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
