using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;

namespace Northwind.Contracts.Interface
{
    public interface IOrderDetailsRepository
    {
        IEnumerable<OrderDetail> GetAllCategory(bool trackChanges);
        OrderDetail GetOrderDetail(int orderId, int productId, bool trackChanges);
        void CreateOrderDetail(OrderDetail orderDetail);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void DeleteOrderDetail(OrderDetail orderDetail);
    }
}
