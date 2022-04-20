using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System;
using System.Collections.Generic;

namespace NorthwindWebApi.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CustomerController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            try
            {
                var customers = _repository.Customer.GetAllCustomer(trackChanges: false);

                var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCustomer)} message : {ex}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "CustomerById")]
        public IActionResult GetCustomerById(string id)
        {
            var customers = _repository.Customer.GetCustomer(id, trackChanges: false);
            if (customers == null)
            {
                _logger.LogInfo($"Category with id : {id} doesn't exist");
                return NotFound();
            }
            else
            {
                var customerDto = _mapper.Map<CustomerDto>(customers);
                return Ok(customerDto);
            }
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                _logger.LogError("Customer object is null");
                return BadRequest("Customer object is null");
            }

            var customerEntity = _mapper.Map<Customer>(customerDto);
            _repository.Customer.CreateCustomer(customerEntity);
            _repository.Save();

            var customerResult = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { id = customerResult.CustomerId }, customerResult);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            var customers = _repository.Customer.GetCustomer(id, trackChanges: false);
            if (customers == null)
            {
                _logger.LogInfo($"Category with id: { id} doesn't exist");
                return NotFound();
            }

            _repository.Customer.DeleteCustomer(customers);
            _repository.Save();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(string id, [FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                _logger.LogError($"Categry must not be null");
                return BadRequest($"Categry must not be null");
            }

            var customerEntity = _repository.Customer.GetCustomer(id, trackChanges: true);
            if (customerEntity == null)
            {
                _logger.LogInfo($"Customer with Id : {id} not found");
                return NotFound();
            }

            var customerResult = _mapper.Map<CustomerDto>(customerEntity);
            _repository.Customer.UpdateCustomer(customerEntity);
            _repository.Save();

            return NoContent();
        }
    }
}
