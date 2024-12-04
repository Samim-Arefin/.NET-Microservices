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
        private readonly ICartCheckOutService _cartCheckOutService;

        public CartController(IRedisCacheService redisCacheService, ICartCheckOutService cartCheckOutService) 
        {
            _redisCacheService = redisCacheService;
            _cartCheckOutService = cartCheckOutService;
        }

        [HttpGet, Route("{key}")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetCarts(string key, CancellationToken cancellationToken) 
            => Ok(await _redisCacheService.GetAsync<ShoppingCartDto>(key, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> UpdateCart(ShoppingCartDto shoppingCart, CancellationToken cancellationToken)
            => Ok(await _cartCheckOutService.UpdateCartItem(shoppingCart, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> Checkout(CartCheckOutDto cartCheckOut, CancellationToken cancellationToken) 
            => Ok(await _cartCheckOutService.CartCheckOut(cartCheckOut, cancellationToken));

        [HttpDelete, Route("{key}")]
        public async Task<IActionResult> RemoveCart(string key, CancellationToken cancellationToken) 
            => Ok(await _redisCacheService.RemoveAsync(key, cancellationToken));
    }
}
