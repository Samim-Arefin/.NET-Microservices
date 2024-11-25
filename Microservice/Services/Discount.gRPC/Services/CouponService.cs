using AutoMapper;
using Discount.gRPC.Entities;
using Shared.API.DtoModels;
using Shared.API.Response;
using System.Net;

namespace Discount.gRPC.Services
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByProductIdAsync(string productId);
        Task<CouponDto> CreateCouponAsync(CouponDto coupon);
        Task<Unit> UpdateCouponAsync(string productId, CouponDto coupon);
        Task<Unit> DeleteCouponAsync(string productId);
    }

    public class CouponService : ICouponService
    {
        public readonly IDapperService _dapperService;
        private readonly IMapper _mapper;
        public CouponService(IDapperService dapperService, IMapper mapper)
        {
            _dapperService = dapperService;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByProductIdAsync(string productId)
        {
            var hasCoupon = await FindCouponAsync(productId);
            if (hasCoupon is null)
            {
                return new()
                {
                    Id = Guid.Empty,
                    ProductName = string.Empty,
                    ProductId = string.Empty,
                    Description = string.Empty,
                    Amount = 0,
                };
            }

            return _mapper.Map<CouponDto>(hasCoupon);
        }

        public async Task<CouponDto> CreateCouponAsync(CouponDto coupon)
        {
            var hasCoupon = await FindCouponAsync(coupon.ProductId);
            if (hasCoupon is not null)
            {
                return coupon;
            }

            var sql = "INSERT INTO COUPON(ProductId, ProductName, Description, Amount) " +
                      "VALUES (@ProductId, @ProductName, @Description, @Amount) RETURNING Id";
            var param = new
            {
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };

            var newCouponId = await _dapperService.CreateAsync<Guid>(sql, param);
            coupon.Id = newCouponId;
            return coupon;
        }

        public async Task<Unit> UpdateCouponAsync(string productId, CouponDto coupon)
        {
            var hasCoupon = await FindCouponAsync(productId);
            if (hasCoupon is null)
            {
                return new(statusCode: (int)HttpStatusCode.BadRequest, isSuccess: false, message: "Coupon not found!");
            }

            var param = new
            {
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };

            var sql = "UPDATE COUPON " +
                      "SET ProductName = @ProductName, Description = @Description, Amount = @Amount " +
                      "WHERE ProductId = @productId";

            var response = await _dapperService.ExecuteAsync(sql, param);

            return response > 0
                   ?
                   new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully updated!")
                   :
                   new(statusCode: (int)HttpStatusCode.BadRequest, isSuccess: false, message: "Coupon not found!");
        }

        public async Task<Unit> DeleteCouponAsync(string productId)
        {
            var coupon = await FindCouponAsync(productId);
            if (coupon is null)
            {
                return new(statusCode: (int)HttpStatusCode.BadRequest, isSuccess: false, message: "Coupon not found!");
            }

            var sql = "DELETE FROM COUPON WHERE ProductId = @productId";
            var response = await _dapperService.ExecuteAsync(sql, new { ProductId = productId });

            return response > 0
                   ?
                   new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully deleted!")
                   :
                   new(statusCode: (int)HttpStatusCode.BadRequest, isSuccess: false, message: "Coupon not found!");
        }

        private async Task<Coupon> FindCouponAsync(string productId)
        {
            var sql = "SELECT * FROM COUPON WHERE ProductId = @productId";
            return await _dapperService.GetAsync<Coupon>(sql, new { ProductId = productId });
        }
    }
}
