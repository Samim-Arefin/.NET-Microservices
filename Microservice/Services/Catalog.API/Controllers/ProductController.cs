using Catalog.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.API.DtoModels;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) 
            => _productService = productService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken) 
            => Ok(await _productService.GetAllAsync<ProductDto>(cancellationToken: cancellationToken));

        [HttpGet, Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken) 
            => Ok(await _productService.GetByIdAsync<ProductDto>(id, cancellationToken));

        [HttpGet, Route("{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken) 
            => Ok(await _productService.GetProductByNameAsync(name, cancellationToken));

        [HttpGet, Route("{categoryName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoryName(string categoryName, CancellationToken cancellationToken) 
            => Ok(await _productService.GetProductByCategoryNameAsync(categoryName, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> InsertOne(ProductDto product, CancellationToken cancellationToken) 
            => Ok(await _productService.InsertOneAsync<ProductDto>(product, cancellationToken: cancellationToken));
        
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> ReplaceOne(string id, ProductDto product, CancellationToken cancellationToken) 
            => Ok(await _productService.ReplaceOneAsync<ProductDto>(id, product, cancellationToken: cancellationToken));

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteOne(string id, CancellationToken cancellationToken) 
            => Ok(await _productService.DeleteOneAsync(id, cancellationToken));

        [HttpDelete]
        public async Task<IActionResult> DeleteMany(CancellationToken cancellationToken) 
            => Ok(await _productService.DeleteManyAsync(cancellationToken: cancellationToken));
    }
}