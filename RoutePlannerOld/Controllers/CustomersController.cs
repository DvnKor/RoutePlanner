﻿using System.Collections.Generic;
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
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customersRepository;
        private readonly IMapper _mapper;

        public CustomersController(CustomerRepository customersRepository, IMapper mapper)
        {
            _customersRepository = customersRepository;
            _mapper = mapper;
        }

       /// <summary>
       /// Получение всех клиентов
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        public IEnumerable<CustomerDto> GetAllCustomers()
        {
            var customers = _customersRepository.GetAllCustomers();
            return customers.Select(customer => _mapper.Map<Customer, CustomerDto>(customer));
        }

       /// <summary>
       /// Дообавление нового клиента
       /// </summary>
       /// <param name="customer"></param>
        [HttpPost]
        public void Post([FromBody] CustomerDto customer)
        {
            _customersRepository.AddCustomer(_mapper.Map<CustomerDto, Customer>(customer));
        }


        /// <summary>
        /// Удаление клиента по customerId
        /// </summary>
        /// <param name="customerId"></param>
        [HttpDelete("{customerId}")]
        public void Delete(int customerId)
        {
            _customersRepository.DeleteCustomer(customerId);
        }
    }
}