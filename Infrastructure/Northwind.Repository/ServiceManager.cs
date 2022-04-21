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

        public Tuple<int, OrderDetail, string> AddToCart(int id, short quantity, string custId, int empId, bool trackChanges)
        {
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            Product product = new Product();
            try
            {
                order = _repository.Orders.GetAllOrders(trackChanges: true)
                        .Where(c => c.CustomerId == custId && c.ShippedDate == null)
                        .SingleOrDefault();

                product = _repository.Products.GetProduct(id, trackChanges: false);
                if (order == null)
                {
                    order = new Order();
                    order.CustomerId = custId;
                    order.OrderDate = DateTime.Now;
                    order.EmployeeId = empId;

                    _repository.Orders.CreateOrders(order);
                    _repository.Save();
                }

                orderDetail = _repository.OrderDetails.GetOrderDetail(order.OrderId, id, trackChanges: true);
                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail();
                    orderDetail.ProductId = id;
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.Quantity = quantity;
                    orderDetail.UnitPrice = (decimal)((decimal)product.UnitPrice * quantity);
                    orderDetail.UnitPrice = orderDetail.UnitPrice;

                    _repository.OrderDetails.CreateOrderDetail(orderDetail);
                    _repository.Save();
                }
                else
                {
                    orderDetail.Quantity = (short)(orderDetail.Quantity + quantity);
                    orderDetail.UnitPrice += (decimal)(product.UnitPrice * quantity);

                    _repository.OrderDetails.UpdateOrderDetail(orderDetail);
                    _repository.Save();
                }

                return Tuple.Create(1, orderDetail, "Success");
            }
            catch (Exception ex)
            {

                return Tuple.Create(-1, orderDetail, ex.Message);
            }
            
        }

        public Tuple<int, Order, string> CheckOut(int id)
        {
            Order orders = new Order();
            try
            {
                var order = _repository.Orders.GetOrders(id, trackChanges: true);
                if (order == null)
                {
                    return Tuple.Create(-1, order, "OrderId Not Found");
                }
                else
                {
                    order.RequiredDate = DateTime.Now;

                    _repository.Orders.UpdateOrders(order);
                    _repository.Save();

                    List<OrderDetail> orderDetail = _repository.OrderDetails.GetAllOrderDetail(trackChanges: true)
                                                .Where(ord => ord.OrderId == id)
                                                .ToList();

                    foreach (var item in orderDetail)
                    {
                        var product = _repository.Products.GetProduct(item.ProductId, trackChanges: true);
                        product.UnitsInStock = (short?)(product.UnitsInStock - item.Quantity);
                        if(product.UnitsInStock < 0)
                        {
                            return Tuple.Create(-2, order, "Out of stock");
                        }

                        _repository.Products.UpdateProduct(product);
                        _repository.Save();
                    }

                    

                    return Tuple.Create(1, order, "Success");
                }
                
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, orders, ex.Message);
            }
        }

        public Tuple<int, IEnumerable<Product>, string> GetAllProduct(bool trackChanges)
        {
            IEnumerable<Product> produst1 = null;
            try
            {
                IEnumerable<Product> product = _repository.Products.GetAllProduct(trackChanges: false);

                return Tuple.Create(1, product, "success");
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, produst1, ex.Message);
            }
            
        }

        public Tuple<int, Order, string> Shipped(ShippedDto shippedDto, int id)
        {
            Order orders = new Order();
            try
            {
                var order = _repository.Orders.GetOrders(id, trackChanges: true);
                var customer = _repository.Customer.GetCustomer(order.CustomerId, trackChanges: true);
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

                    _repository.Orders.UpdateOrders(order);
                    _repository.Save();

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
