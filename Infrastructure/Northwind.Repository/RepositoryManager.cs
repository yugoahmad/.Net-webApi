using Northwind.Contracts;
using Northwind.Contracts.Interface;
using Northwind.Entities.Contexts;
using Northwind.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private ICategoryRepository _categoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderDetailsRepository _orderDetailsRepository;
        private IOrdersRepository _ordersRepository;
        private IProductRepository _productRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICategoryRepository Category
        {
            get {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_repositoryContext);
                }
                return _categoryRepository; 
            }
        }

        public ICustomerRepository Customer
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(_repositoryContext);
                }
                return _customerRepository;
            }
        }

        public IOrderDetailsRepository OrderDetails
        {
            get
            {
                if (_orderDetailsRepository == null)
                {
                    _orderDetailsRepository = new OrderDetailsRepository(_repositoryContext);
                }
                return _orderDetailsRepository;
            }
        }

        public IOrdersRepository Orders
        {
            get
            {
                if (_ordersRepository == null)
                {
                    _ordersRepository = new OrderRepository(_repositoryContext);
                }
                return _ordersRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_repositoryContext);
                }
                return _productRepository;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
