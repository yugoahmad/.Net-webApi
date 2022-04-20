using Northwind.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategory(bool trackChanges);
        Category GetCategory(int id, bool trackChanges);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
