using AutoMapper;
using Ordering.Application.Commands;
using Shared.API.Events;

namespace Ordering.API.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CartCheckOutEvent, CheckoutOrderCommand>();
        }
    }
}
