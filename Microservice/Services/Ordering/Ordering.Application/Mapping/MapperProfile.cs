using AutoMapper;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Order, OrderResponse>();
            CreateMap<CheckoutOrderCommand, Order>();
            CreateMap<UpdateOrderCommand, Order>();
        }
    }
}
