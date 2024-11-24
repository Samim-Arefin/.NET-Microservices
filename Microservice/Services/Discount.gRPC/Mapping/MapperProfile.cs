using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Shared.API.DtoModels;

namespace Discount.gRPC.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CouponDto, CouponResponse>();
            CreateMap<Coupon, CouponDto>();
            CreateMap<CouponResponse, CouponDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
