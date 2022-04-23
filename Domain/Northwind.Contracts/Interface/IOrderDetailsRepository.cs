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
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync(bool trackChanges);
        Task<OrderDetail> GetOrderDetailAsync(int orderId, int productId, bool trackChanges);
        void CreateOrderDetailAsync(OrderDetail orderDetail);
        void UpdateOrderDetailAsync(OrderDetail orderDetail);
        void DeleteOrderDetailAsync(OrderDetail orderDetail);
    }
}
