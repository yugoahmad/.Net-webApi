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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        void ICategoryRepository.CreateCategory(Category category)
        {
            Create(category);
        }

        void ICategoryRepository.DeleteCategory(Category category)
        {
            Delete(category);
        }

        IEnumerable<Category> ICategoryRepository.GetAllCategory(bool trackChanges) =>
            FindAll(trackChanges)
                .OrderBy(c => c.CategoryName)
                .ToList();

        Category ICategoryRepository.GetCategory(int id, bool trackChanges) =>
            FindByCondition(c => c.CategoryId.Equals(id), trackChanges).SingleOrDefault();

        void ICategoryRepository.UpdateCategory(Category category)
        {
            Update(category);
        }
    }
}
