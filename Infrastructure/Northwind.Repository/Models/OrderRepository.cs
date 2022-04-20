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

        public void CreateOrders(Order order)
        {
            Create(order);
        }

        public void DeleteOrders(Order order)
        {
            Delete(order);
        }

        public IEnumerable<Order> GetAllOrders(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(o => o.OrderId)
            .ToList();

        public Order GetOrders(int id, bool trackChanges) =>
            FindByCondition(o => o.OrderId.Equals(id), trackChanges).SingleOrDefault();

        public void UpdateOrders(Order order)
        {
            Update(order);
        }
    }
}
