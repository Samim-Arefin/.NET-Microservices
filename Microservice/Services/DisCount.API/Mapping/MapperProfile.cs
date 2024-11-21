using AutoMapper;
using Discount.API.Entities;
using Shared.API.DtoModels;

namespace Discount.API.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Coupon, CouponDto>();
        }
    }
}
