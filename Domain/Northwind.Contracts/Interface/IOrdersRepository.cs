using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts.Interface
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        Task<Order> GetOrdersAsync(int id, bool trackChanges);
        void CreateOrdersAsync(Order order);
        void UpdateOrdersAsync(Order order);
        void DeleteOrdersAsync(Order order);
    }
}
