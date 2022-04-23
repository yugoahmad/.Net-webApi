using Northwind.Contracts;
using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repository
{
    public class ServiceManager : IServiceManager
    {
        private readonly IRepositoryManager _repository;

        public ServiceManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Tuple<int, OrderDetail, string>> AddToCart(CartDto cartDto, bool trackChanges)
        {
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            Product product = new Product();
            try
            {
                order =  _repository.Orders.GetAllOrdersAsync(trackChanges: true)
                        .Result.Where(c => c.CustomerId == cartDto.CustomerId && c.ShippedDate == null)
                        .FirstOrDefault();

                product =  await _repository.Products.GetProductAsync(cartDto.ProductId, trackChanges: false);
                if (order == null)
                {
                    order = new Order();
                    order.CustomerId = cartDto.CustomerId;
                    order.OrderDate = DateTime.Now;
                    order.EmployeeId = cartDto.EmployeeId;

                    _repository.Orders.CreateOrdersAsync(order);
                    await _repository.SaveAsync();
                }

                orderDetail = await _repository.OrderDetails.GetOrderDetailAsync(order.OrderId, cartDto.ProductId, trackChanges: true);
                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail();
                    orderDetail.ProductId = cartDto.ProductId;
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.Quantity = cartDto.Quantity;
                    orderDetail.UnitPrice = (decimal)((decimal)product.UnitPrice * cartDto.Quantity);
                    orderDetail.UnitPrice = orderDetail.UnitPrice;

                    _repository.OrderDetails.CreateOrderDetailAsync(orderDetail);
                    await _repository.SaveAsync();
                }
                else
                {
                    orderDetail.Quantity = (short)(orderDetail.Quantity + cartDto.Quantity);
                    orderDetail.UnitPrice += (decimal)(product.UnitPrice * cartDto.Quantity);

                    _repository.OrderDetails.UpdateOrderDetailAsync(orderDetail);
                    await _repository.SaveAsync();
                }

                return Tuple.Create(1, orderDetail, "Success");
            }
            catch (Exception ex)
            {

                return Tuple.Create(-1, orderDetail, ex.Message);
            }
            
        }

        public async Task<Tuple<int, Order, string>> CheckOut(int id)
        {
            Order orders = new Order();
            try
            {
                var order = await _repository.Orders.GetOrdersAsync(id, trackChanges: true);
                if (order == null)
                {
                    return Tuple.Create(-1, order, "OrderId Not Found");
                }
                else
                {
                    order.RequiredDate = DateTime.Now;

                    _repository.Orders.UpdateOrdersAsync(order);
                    await _repository.SaveAsync();

                    List<OrderDetail> orderDetail = _repository.OrderDetails.GetAllOrderDetailAsync(trackChanges: true)
                                                .Result.Where(ord => ord.OrderId == id)
                                                .ToList();

                    foreach (var item in orderDetail)
                    {
                        var product = await _repository.Products.GetProductAsync(item.ProductId, trackChanges: true);
                        product.UnitsInStock = (short?)(product.UnitsInStock - item.Quantity);
                        if(product.UnitsInStock < 0)
                        {
                            return Tuple.Create(-2, order, "Out of stock");
                        }

                        _repository.Products.UpdateProductAsync(product);
                        await _repository.SaveAsync();
                    }

                    return Tuple.Create(1, order, "Success");
                }
                
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, orders, ex.Message);
            }
        }

        public async Task<Tuple<int, IEnumerable<Product>, string>> GetAllProduct(bool trackChanges)
        {
            IEnumerable<Product> produst1 = null;
            try
            {
                IEnumerable<Product> product = await _repository.Products.GetAllProductAsync(trackChanges: false);

                return Tuple.Create(1, product, "success");
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, produst1, ex.Message);
            }
            
        }

        public async Task<Tuple<int, Order, string>> Shipped(ShippedDto shippedDto, int id)
        {
            Order orders = new Order();
            try
            {
                var order = await _repository.Orders.GetOrdersAsync(id, trackChanges: true);
                var customer = await _repository.Customer.GetCustomerAsync(order.CustomerId, trackChanges: true);
                if (order == null)
                {
                    return Tuple.Create(-1, order, "OrderId Not Found");
                }
                else
                {
                    order.ShipAddress = customer.Address;
                    order.ShipCity = customer.City;
                    order.ShipRegion = customer.Region;
                    order.ShipPostalCode = customer.PostalCode;
                    order.ShipCountry = customer.Country;
                    order.ShipVia = shippedDto.ShipVia;
                    order.Freight = shippedDto.Freight;
                    order.ShipName = shippedDto.ShipName;
                    order.ShippedDate = shippedDto.ShippedDate;

                    _repository.Orders.UpdateOrdersAsync(order);
                    await _repository.SaveAsync();

                    return Tuple.Create(1, order, "Success");
                }
            }
            catch (Exception ex)
            {

                return Tuple.Create(-1, orders, ex.Message);
            }
        }
    }
}
