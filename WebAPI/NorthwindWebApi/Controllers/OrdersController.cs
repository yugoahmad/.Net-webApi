using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateOrder([FromBody] CartDto cartDto)
        {
            var orderDetail = await _service.AddToCart(cartDto, trackChanges: true);
            
            return Ok(_mapper.Map<OrderDetailsDto>(orderDetail.Item2));

        }

        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var order = await _service.CheckOut(id);
            if (order.Item1 == -2)
            {
                return BadRequest(order.Item3);
            }

            if (order.Item1 == -1)
            {
                return BadRequest(order.Item3);
            }
            else
            {
                return Ok(_mapper.Map<OrdersDto>(order.Item2));
            }
        }

        [HttpPost("shipped/{id}")]
        public async Task<IActionResult> ShipOrder([FromBody] ShippedDto shippedDto, int id)
        {
            var shipped = await _service.Shipped(shippedDto, id);
            if (shipped.Item1 == -1)
            {
                return BadRequest(shipped.Item3);
            }
            else
            {
                return Ok(_mapper.Map<OrdersDto>(shipped.Item2));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            try
            {
                var product = await _service.GetAllProduct(trackChanges: false);
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(product.Item2));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
    }
}
