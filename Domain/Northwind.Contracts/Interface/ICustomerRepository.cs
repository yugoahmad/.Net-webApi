using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts.Interface
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomer(bool trackChanges);
        Customer GetCustomer(int id, bool trackChanges);
        Customer GetCustomer(string id, bool trackChanges);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
