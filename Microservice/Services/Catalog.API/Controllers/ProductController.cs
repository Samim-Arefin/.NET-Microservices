using Catalog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.API.DtoModels;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetProducts() => 
            Ok(await _productService.GetAllAsync<ProductDto>());

        [HttpGet, Route("{id}")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetById(string id) => 
            Ok(await _productService.GetByIdAsync<ProductDto>(id));

        [HttpGet, Route("{name}")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetByName(string name) =>
            Ok(await _productService.GetProductByNameAsync(name));

        [HttpGet, Route("{categoryName}")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetByCategoryName(string categoryName) =>
         Ok(await _productService.GetProductByCategoryNameAsync(categoryName));

        [HttpPost]
        public async Task<IActionResult> InsertOne(ProductDto product) => 
            Ok(await _productService.InsertOneAsync<ProductDto>(product));
        
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> ReplaceOne(string id, ProductDto product) => 
            Ok(await _productService.ReplaceOneAsync<ProductDto>(id, product));

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteOne(string id) =>
            Ok(await _productService.DeleteOneAsync(id));

        [HttpDelete]
        public async Task<IActionResult> DeleteMany() =>
            Ok(await _productService.DeleteManyAsync());
    }
}