using Shared.API.DtoModels;
using Shared.API.Response;

namespace Cart.API.Services.Interfaces
{
    public interface ICartCheckOutService
    {
        Task<Unit> UpdateCartItem(ShoppingCartDto shoppingCart, CancellationToken cancellationToken = default);
        Task<Unit> CartCheckOut(CartCheckOutDto cartCheckOut, CancellationToken cancellationToken = default);
    }
}
