using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts.Interface;

namespace Northwind.Contracts
{
    public interface IRepositoryManager
    {
        ICategoryRepository Category { get; }
        ICustomerRepository Customer { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrdersRepository Orders { get; }
        IProductRepository Products { get; }
        void Save();
        Task SaveAsync();
    }
}
