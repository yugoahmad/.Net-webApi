using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public void CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            Create(orderDetail);
        }

        public void DeleteOrderDetailAsync(OrderDetail orderDetail)
        {
            Delete(orderDetail);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(o => o.OrderId)
            .ToListAsync();

        public async Task<OrderDetail> GetOrderDetailAsync(int orderId, int productId, bool trackChanges) =>
            await FindByCondition(o => o.OrderId.Equals(orderId) && o.ProductId.Equals(productId), trackChanges).SingleOrDefaultAsync();

        public void UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            Update(orderDetail);
        }
    }
}
