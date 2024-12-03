using AutoMapper;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using System.Net;

namespace Ordering.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, UnitResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<UnitResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            await _orderRepository.UpdateOrder(order);
            return new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully updated!");
        }
    }
}
