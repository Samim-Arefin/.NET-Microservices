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
        private readonly ILogger _logger;
        public ProductService(IDbContext context, ILogger<Product> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _logger = logger;
        }

        public async Task<ProductDto?> GetProductByNameAsync(string name)
        {
            try
            {
                Expression<Func<Product, bool>> filter = product => string.Equals(product.Name, name, StringComparison.OrdinalIgnoreCase);
                var product = await FindOneAsync<ProductDto>(filter);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<ProductDto>?> GetProductByCategoryNameAsync(string categoryName)
        {
            try
            {
                Expression<Func<Product, bool>> filter = product => string.Equals(product.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase);
                var product = await GetAllAsync<ProductDto>(filter);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new();
            }
        }
    }
}
