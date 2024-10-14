using Catalog.API.Entities;
using Shared.API.DtoModels;

namespace Catalog.API.Services.Interfaces
{
    public interface IProductService : ICommonService<Product> 
    {
        Task<ProductDto?> GetProductByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<ProductDto>?> GetProductByCategoryNameAsync(string categoryName, CancellationToken cancellationToken = default);
    }
}
