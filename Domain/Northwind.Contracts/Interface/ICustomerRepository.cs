using Northwind.Entities.Models;
using Northwind.Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomerAsync(bool trackChanges);
        //Task<Customer> GetCustomerAsync(int id, bool trackChanges);
        Task<Customer> GetCustomerAsync(string id, bool trackChanges);
        void CreateCustomerAsync(Customer customer);
        void UpdateCustomerAsync(Customer customer);
        void DeleteCustomerAsync(Customer customer);
        Task<IEnumerable<Customer>> GetPaginationCustomerAsync(CustomerParameters customerParameters, bool trackChanges);
        Task<IEnumerable<Customer>> SearchCustomer(CustomerParameters customerParameters, bool trackChanges);
    }
}
