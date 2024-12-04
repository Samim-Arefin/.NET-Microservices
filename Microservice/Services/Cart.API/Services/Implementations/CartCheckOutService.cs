using AutoMapper;
using Cart.API.Services.Interfaces;
using Shared.API.DtoModels;
using Shared.API.Events;
using Shared.API.Response;
using System.Net;

namespace Cart.API.Services.Implementations
{
    public class CartCheckOutService : ICartCheckOutService
    {
        private readonly IDiscountgRPCService _discountgRPCService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IEventBusService _eventBusService;
        private readonly IMapper _mapper;
        public CartCheckOutService(IDiscountgRPCService discountgRPCService, IRedisCacheService redisCacheService, IEventBusService eventBusService, IMapper mapper)
        {
            _discountgRPCService = discountgRPCService;
            _redisCacheService = redisCacheService;
            _eventBusService = eventBusService;
            _mapper = mapper;
        }

        public async Task<Unit> CartCheckOut(CartCheckOutDto cartCheckOut, CancellationToken cancellationToken = default)
        {
            var cart = await _redisCacheService.GetAsync<ShoppingCartDto>(cartCheckOut.UserName);

            if (cart is null)
            {
                return new(statusCode: (int)HttpStatusCode.BadRequest, isSuccess: false, message: "Cart not found!");
            }

            var eventMessage = _mapper.Map<CartCheckOutEvent>(cartCheckOut);

            await _eventBusService.PublishAsync<CartCheckOutEvent>(eventMessage);

            await _redisCacheService.RemoveAsync(cart.UserName);

            return new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Order has been placed.");
        }

        public async Task<Unit> UpdateCartItem(ShoppingCartDto shoppingCart, CancellationToken cancellationToken = default)
        {
            foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                var coupon = await _discountgRPCService.GetDiscount(shoppingCartItem.ProductId);
                shoppingCartItem.Price -= coupon.Amount;
            }

           var response = await _redisCacheService.SetAsync<ShoppingCartDto>(shoppingCart.UserName, shoppingCart, cancellationToken);
           return response;
        }
    }
}
