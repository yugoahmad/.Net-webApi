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

        public IEnumerable<OrderDetail> GetAllCategory(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(o => o.OrderId)
            .ToList();

        public OrderDetail GetOrderDetail(int id, bool trackChanges) =>
            FindByCondition(o => o.OrderId.Equals(id), trackChanges).SingleOrDefault();

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            Update(orderDetail);
        }
    }
}
