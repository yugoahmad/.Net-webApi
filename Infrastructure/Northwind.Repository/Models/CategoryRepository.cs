using Microsoft.EntityFrameworkCore;
using Northwind.Contracts.Interface;
using Northwind.Entities.Contexts;
using Northwind.Entities.Models;
using Northwind.Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repository.Models
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Category>> GetPaginationCategoryAsync(CategoryParameters categoryParameters, bool trackChanges)
        {
            return await FindAll(trackChanges)
                         .OrderBy(c => c.CategoryName)
                         .Skip((categoryParameters.PageNumber - 1) * categoryParameters.PageSize)
                         .Take(categoryParameters.PageSize)
                         .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSearchCategory(CategoryParameters categoryParameters, bool trackChanges)
        {
            if (string.IsNullOrWhiteSpace(categoryParameters.SearchCategory))
            {
                return await FindAll(trackChanges).ToListAsync();
            }

            var lowerToCase = categoryParameters.SearchCategory.Trim().ToLower();
            return await FindAll(trackChanges)
                         .Where(c => c.CategoryName.ToLower().Contains(lowerToCase))
                         .Include(c => c.Products)
                         .OrderBy(c => c.CategoryName)
                         .ToListAsync();
        }

        void ICategoryRepository.CreateCategoryAsync(Category category)
        {
            Create(category);
        }

        void ICategoryRepository.DeleteCategoryAsync(Category category)
        {
            Delete(category);
        }

        async Task<IEnumerable<Category>> ICategoryRepository.GetAllCategoryAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(c => c.CategoryName)
                  .ToListAsync();

        async Task<Category> ICategoryRepository.GetCategoryAsync(int id, bool trackChanges) =>
            await FindByCondition(c => c.CategoryId.Equals(id), trackChanges).SingleOrDefaultAsync();

        void ICategoryRepository.UpdateCategoryAsync(Category category)
        {
            Update(category);
        }
    }
}
