using Cart.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.API.DtoModels;

namespace Cart.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        public CartController(IRedisCacheService redisCacheService) => 
            _redisCacheService = redisCacheService;

        [HttpGet, Route("key")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetCarts(string key, CancellationToken cancellationToken) =>
            Ok(await _redisCacheService.GetAsync<ShoppingCartDto>(key, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> UpdateCart(ShoppingCartDto shoppingCart, CancellationToken cancellationToken) =>
            Ok(await _redisCacheService.SetAsync<ShoppingCartDto>(shoppingCart.UserName, shoppingCart, cancellationToken));

        [HttpDelete, Route("key")]
        public async Task<IActionResult> RemoveCart(string key, CancellationToken cancellationToken) =>
            Ok(await _redisCacheService.RemoveAsync(key, cancellationToken));
    }
}
