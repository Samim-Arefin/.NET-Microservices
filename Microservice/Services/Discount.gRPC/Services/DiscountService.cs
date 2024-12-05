using AutoMapper;
using Discount.gRPC.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Shared.API.DtoModels;

namespace Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;
        public DiscountService(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public override async Task<CouponResponse> GetDiscount(DiscountRequest request, ServerCallContext context)
        {
            var coupon = await _couponService.GetCouponByProductIdAsync(request.ProductId);
            return _mapper.Map<CouponResponse>(coupon);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<CouponResponse> CreateDiscount(CouponRequest request, ServerCallContext context)
        {
            var coupon = await _couponService.CreateCouponAsync(_mapper.Map<CouponDto>(request));
            return _mapper.Map<CouponResponse>(coupon);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<UnitResponse> UpdateDiscount(CouponRequest request, ServerCallContext context)
        {
            var response = await _couponService.UpdateCouponAsync(request.ProductId, _mapper.Map<CouponDto>(request));
            return _mapper.Map<UnitResponse>(response);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<UnitResponse> DeleteDiscount(DiscountRequest request, ServerCallContext context)
        {
            var response = await _couponService.DeleteCouponAsync(request.ProductId);
            return _mapper.Map<UnitResponse>(response);
        }
    }
}
