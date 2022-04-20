using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace NorthwindWebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public OrdersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IServiceManager service)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpPost("addToCart")]
        public IActionResult CreateOrder([FromBody] CartDto cartDto)
        {
            var orderDetail = _service.AddToCart(cartDto.ProductId, cartDto.Quantity, cartDto.CustomerId, cartDto.EmployeeId, trackChanges: true);
            
            return Ok(_mapper.Map<OrderDetailsDto>(orderDetail.Item2));

        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            try
            {
                var product = _service.GetAllProduct(trackChanges: false);
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(product.Item2));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
    }
}
