using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Services.Implementation;
using Catalog.API.Services.Interfaces;
using Shared.API.DtoModels;
using System.Linq.Expressions;

namespace Catalog.API.Services.Implementations
{
    public class ProductService : CommonService<Product>, IProductService
    {
        public ProductService(IDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<ProductDto?> GetProductByNameAsync(string name)
        {
            Expression<Func<Product, bool>> filter = product => string.Equals(product.Name, name, StringComparison.OrdinalIgnoreCase);
            var product = await FindOneAsync<ProductDto>(filter);
            return product;
        }

        public async Task<List<ProductDto>?> GetProductByCategoryNameAsync(string categoryName)
        {
            Expression<Func<Product, bool>> filter = product => string.Equals(product.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase);
            var product = await GetAllAsync<ProductDto>(filter);
            return product;
        }
    }
}
