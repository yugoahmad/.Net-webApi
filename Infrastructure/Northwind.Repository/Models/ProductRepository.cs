using Microsoft.EntityFrameworkCore;
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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateProductAsync(Product product)
        {
            Create(product);
        }

        public void DeleteProductAsync(Product product)
        {
            Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        public async Task<Product> GetProductAsync(int id, bool trackChanges) =>
            await FindByCondition(p => p.ProductId.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void UpdateProductAsync(Product product)
        {
            Update(product);
        }
    }
}
