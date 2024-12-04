using AutoMapper;
using Shared.API.DtoModels;
using Shared.API.Events;

namespace Cart.API.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CartCheckOutDto, CartCheckOutEvent>();
        }
    }
}
