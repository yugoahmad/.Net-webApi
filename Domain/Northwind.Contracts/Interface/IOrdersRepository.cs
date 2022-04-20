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
        IEnumerable<Order> GetAllOrders(bool trackChanges);
        Order GetOrders(int id, bool trackChanges);
        void CreateOrders(Order order);
        void UpdateOrders(Order order);
        void DeleteOrders(Order order);
    }
}
