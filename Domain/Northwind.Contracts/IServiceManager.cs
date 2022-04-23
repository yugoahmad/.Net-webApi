using Northwind.Entities.DataTransferObject;
using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts
{
    public interface IServiceManager
    {
        Task<Tuple<int, IEnumerable<Product>, string>> GetAllProduct(bool trackChanges);
        Task<Tuple<int, OrderDetail, string>> AddToCart(CartDto cartDto, bool trackChanges);
        Task<Tuple<int, Order, string>> CheckOut(int id);
        Task<Tuple<int, Order, string>> Shipped(ShippedDto shippedDto, int id);
    }
}
