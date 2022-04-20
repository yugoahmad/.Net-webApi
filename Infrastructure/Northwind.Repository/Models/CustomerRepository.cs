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
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCustomer(Customer customer)
        {
            Create(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            Delete(customer);
        }

        public IEnumerable<Customer> GetAllCustomer(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.ContactName)
            .ToList();

        public Customer GetCustomer(int id, bool trackChanges) =>
            FindByCondition(c => c.CustomerId.Equals(id), trackChanges).SingleOrDefault();

        public Customer GetCustomer(string id, bool trackChanges) =>
            FindByCondition(c => c.CustomerId.Equals(id), trackChanges).SingleOrDefault();

        public void UpdateCustomer(Customer customer)
        {
            Update(customer);
        }
    }
}
