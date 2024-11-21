using Discount.API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.API.DtoModels;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService) 
            => _couponService = couponService;

        [HttpGet, Route("{productId}")]
        [ResponseCache(Duration = 3)]
        public async Task<IActionResult> GetCoupon(string productId) 
            => Ok(await _couponService.GetCouponByProductIdAsync(productId));

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto coupon)
            => Ok(await _couponService.CreateCouponAsync(coupon));

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdateCoupon(Guid id, CouponDto coupon)
            => Ok(await _couponService.UpdateCouponAsync(id, coupon));

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteCoupon(Guid id)
            => Ok(await _couponService.DeleteCouponAsync(id));

    }
}
