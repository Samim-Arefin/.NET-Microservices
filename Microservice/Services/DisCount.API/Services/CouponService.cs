using AutoMapper;
using Discount.API.Entities;
using Shared.API.DtoModels;
using Shared.API.Response;

namespace Discount.API.Services
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByProductIdAsync(string productId);
        Task<CouponDto> CreateCouponAsync(CouponDto coupon);
        Task<Unit> UpdateCouponAsync(Guid id, CouponDto coupon);
        Task<Unit> DeleteCouponAsync(Guid id);
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
            var sql = "SELECT * FROM COUPON WHERE ProductId = @ProductId";
            var coupon = await _dapperService.GetAsync<Coupon>(sql, new { ProductId = productId });
            if (coupon is null)
            {
                return new()
                {
                    ProductName = "No discount!",
                    Amount = 0,
                };
            }
            
            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto> CreateCouponAsync(CouponDto coupon)
        {
            var sql = "INSERT INTO COUPON(ProductId, ProductName, Description, Amount) VALUES (@ProductId, @ProductName, @Description, @Amount) RETURNING Id";
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

        public async Task<Unit> UpdateCouponAsync(Guid id, CouponDto coupon)
        {
            var hasCoupon = await FindCouponAsync(id);
            if (hasCoupon is null)
            {
                return new(statusCode: HttpStatusCodes.OK, isSuccess: false, message: "Coupon not found!");
            }

            var param = new
            {
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };

            var sql = "UPDATE COUPON " +
                      "SET ProductId = @ProductId, ProductName = @ProductName, Description = @Description, Amount = @Amount " +
                      "WHERE Id = @Id";
            var response = await _dapperService.ExecuteAsync(sql, param);
            return new(statusCode: HttpStatusCodes.OK, isSuccess: true, message: "Successfully updated!");
        }

        public async Task<Unit> DeleteCouponAsync(Guid id)
        {
            var coupon = await FindCouponAsync(id);
            if(coupon is null)
            {
                return new(statusCode: HttpStatusCodes.OK, isSuccess: false, message: "Coupon not found!");
            }

            var sql = "DELETE FROM COUPON WHERE Id = @Id";
            var response = await _dapperService.ExecuteAsync(sql, new { Id = id });
            return new(statusCode: HttpStatusCodes.OK, isSuccess: true, message: "Successfully deleted!");
        }

        private async Task<Coupon> FindCouponAsync(Guid id)
        {
            var sql = "SELECT * FROM COUPON WHERE Id = @Id";
            return await _dapperService.GetAsync<Coupon>(sql, new { Id = id });
        }
    }
}
