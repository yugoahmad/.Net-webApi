using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using Northwind.Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var customers = await _repository.Customer.GetAllCustomerAsync(trackChanges: false);

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
        public async Task<IActionResult> GetCustomerById(string id)
        {
            var customers = await _repository.Customer.GetCustomerAsync(id, trackChanges: false);
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
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                _logger.LogError("Customer object is null");
                return BadRequest("Customer object is null");
            }

            //object modelstate digunakan untuk validasi data yang ditangkap oleh customerdto
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid modelstate customerDto");
                return UnprocessableEntity(ModelState);
            }

            var customerEntity = _mapper.Map<Customer>(customerDto);
            _repository.Customer.CreateCustomerAsync(customerEntity);
            await _repository.SaveAsync();

            var customerResult = _mapper.Map<CustomerDto>(customerEntity);
            return CreatedAtRoute("CustomerById", new { id = customerResult.CustomerId }, customerResult);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _repository.Customer.GetCustomerAsync(id, trackChanges: false);
            if (customer == null)
            {
                _logger.LogInfo($"Customer with id : {id} doesn't exist in database");
                return NotFound();
            }

            _repository.Customer.DeleteCustomerAsync(customer);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] CustomerUpdateDto customerDto)
        {
            if (customerDto == null)
            {
                _logger.LogError("Customer must not be null");
                return BadRequest("Customer must not be null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for customerdto object");
                return UnprocessableEntity(ModelState);
            }
            var customerEntity = await _repository.Customer.GetCustomerAsync(id, trackChanges: true);

            if (customerEntity == null)
            {
                _logger.LogError($"Company with id : {id} not found");
                return NotFound();
            }

            _mapper.Map(customerDto, customerEntity);
            //_repository.Customer.UpdateCustomer(customerEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCustomer(string id, [FromBody]
                             JsonPatchDocument<CustomerUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError($"PatchDoc object sent is null");
                return BadRequest("PatchDoc object sent is null");
            }

            var customerEntity = await _repository.Customer.GetCustomerAsync(id, trackChanges: true);

            if (customerEntity == null)
            {
                _logger.LogError($"Customer with id : {id} not found");
                return NotFound();
            }

            var customerPatch = _mapper.Map<CustomerUpdateDto>(customerEntity);

            patchDoc.ApplyTo(customerPatch);

            TryValidateModel(customerPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for pathc document");
                return UnprocessableEntity();
            }

            _mapper.Map(customerPatch, customerEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] CustomerParameters customerParameters)
        {
            var customerPage = await _repository.Customer.GetPaginationCustomerAsync(customerParameters, trackChanges: false);

            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customerPage);
            return Ok(customerDto);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetCustomerSearch([FromQuery] CustomerParameters customerParameters)
        {
            var customerSearch = await _repository.Customer.SearchCustomer(customerParameters, trackChanges: false);

            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(customerSearch);
            return Ok(customerDto);
        }
    }
}
