using Microsoft.EntityFrameworkCore;
using Northwind.Contracts.Interface;
using Northwind.Entities.Contexts;
using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repository.Models
{
    public class OrderRepository : RepositoryBase<Order>, IOrdersRepository
    {
        public OrderRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOrdersAsync(Order order)
        {
            Create(order);
        }

        public void DeleteOrdersAsync(Order order)
        {
            Delete(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(o => o.OrderId)
            .ToListAsync();

        public async Task<Order> GetOrdersAsync(int id, bool trackChanges) =>
            await FindByCondition(o => o.OrderId.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void UpdateOrdersAsync(Order order)
        {
            Update(order);
        }
    }
}
