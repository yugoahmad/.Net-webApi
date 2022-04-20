using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts
{
    public interface IServiceManager
    {
        Tuple<int, IEnumerable<Product>, string> GetAllProduct(bool trackChanges);
        Tuple<int, OrderDetail, string> AddToCart(int id, short quantity, string custId, int empId, bool trackChanges);
    }
}
