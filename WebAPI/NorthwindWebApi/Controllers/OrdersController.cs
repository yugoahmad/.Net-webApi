using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System.Linq;
using System;

namespace NorthwindWebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public OrdersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("addToCart")]
        public IActionResult CreateOrder([FromBody] OrderDetailsDto orderDetailDto)
        {
            Order orders = new Order();
            Product product = new Product();
            Customer customer = new Customer();
            OrderDetail orderDetail = new OrderDetail();

            var order = _repository.Orders.GetAllOrders(trackChanges: true).Where(c => c.CustomerId == orderDetailDto.CustomerId && c.ShippedDate == null).SingleOrDefault();
            var productId = _repository.Products.GetProduct(orderDetailDto.ProductId, trackChanges: true);
            var customerId = _repository.Customer.GetCustomer(orderDetailDto.CustomerId, trackChanges: true);
            if(order == null)
            {
                order.CustomerId = orderDetailDto.CustomerId;
                order.OrderDate = DateTime.Now;

                _repository.Orders.CreateOrders(order);
                _repository.Save();
            }
            if(orderDetail == null)
            {
                orderDetail.ProductId = orderDetailDto.ProductId;
                orderDetail.OrderId = order.OrderId;
                orderDetail.Quantity = orderDetailDto.Quantity;

                _repository.OrderDetails.CreateOrderDetail(orderDetail);
                _repository.Save();
            }
            else
            {
                orderDetail.Quantity = (short)(orderDetail.Quantity + orderDetailDto.Quantity);

                _repository.OrderDetails.UpdateOrderDetail(orderDetail);
                _repository.Save();
            }



        }
    }
}
