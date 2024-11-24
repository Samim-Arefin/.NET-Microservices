using Cart.API.Services.Interfaces;
using Discount.gRPC.Protos;

namespace Cart.API.Services.Implementations
{
    public class DiscountgRPCService : IDiscountgRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;
        public DiscountgRPCService(DiscountProtoService.DiscountProtoServiceClient client) 
            => _client = client;

        public async Task<CouponResponse> GetDiscount(string productId)
        {
            DiscountRequest discountRequest = new()
            { 
                ProductId = productId 
            };

           return await _client.GetDiscountAsync(discountRequest);
        }
    }
}
