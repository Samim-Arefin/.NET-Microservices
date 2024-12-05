using AutoMapper;
using MediatR;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace Ordering.Application.Handlers
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrderResponse>>(orders);
        }
    }
}
