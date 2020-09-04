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
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customersRepository;
        private readonly IMapper _mapper;

        public CustomersController(CustomerRepository customersRepository, IMapper mapper)
        {
            _customersRepository = customersRepository;
            _mapper = mapper;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public IEnumerable<CustomerDto> GetAllCustomers()
        {
            var customers = _customersRepository.GetAllCustomers();
            return customers.Select(customer => _mapper.Map<Customer, CustomerDto>(customer));
        }

        // POST api/<CustomersController>
        [HttpPost]
        public void Post([FromBody] CustomerDto customer)
        {
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}