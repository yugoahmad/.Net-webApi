using Northwind.Entities.Models;
using Northwind.Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Contracts.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync(bool trackChanges);
        Task<Category> GetCategoryAsync(int id, bool trackChanges);
        void CreateCategoryAsync(Category category);
        void UpdateCategoryAsync(Category category);
        void DeleteCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetPaginationCategoryAsync(CategoryParameters categoryParameters, bool trackChanges);
        Task<IEnumerable<Category>> GetSearchCategory(CategoryParameters categoryParameters, bool trackChanges);
    }
}
