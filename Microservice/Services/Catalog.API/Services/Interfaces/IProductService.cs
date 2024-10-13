using Catalog.API.Entities;
using Shared.API.DtoModels;

namespace Catalog.API.Services.Interfaces
{
    public interface IProductService : ICommonService<Product> 
    {
        Task<ProductDto?> GetProductByNameAsync(string name);
        Task<List<ProductDto>?> GetProductByCategoryNameAsync(string categoryName);
    }
}
