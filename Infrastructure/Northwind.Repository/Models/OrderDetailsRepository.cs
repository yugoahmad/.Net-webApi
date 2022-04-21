using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts.Interface;
using Northwind.Entities.Contexts;
using Northwind.Entities.Models;

namespace Northwind.Repository.Models
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetail>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            Create(orderDetail);
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            Delete(orderDetail);
        }

        public IEnumerable<OrderDetail> GetAllOrderDetail(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(o => o.OrderId)
            .ToList();

        public OrderDetail GetOrderDetail(int orderId, int productId, bool trackChanges) =>
            FindByCondition(o => o.OrderId.Equals(orderId) && o.ProductId.Equals(productId), trackChanges).SingleOrDefault();

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            Update(orderDetail);
        }
    }
}
