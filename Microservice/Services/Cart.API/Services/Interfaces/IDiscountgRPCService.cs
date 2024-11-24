using Discount.gRPC.Protos;

namespace Cart.API.Services.Interfaces
{
    public interface IDiscountgRPCService
    {
        Task<CouponResponse> GetDiscount(string productId);
    }
}
