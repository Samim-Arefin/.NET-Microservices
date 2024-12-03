using AutoMapper;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Notifications;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublisher _publisher;
        private readonly IMapper _mapper;
        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IPublisher publisher, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _publisher = publisher;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddOrder(order);
            if(newOrder is not null)
            {
                var to = newOrder.EmailAddress;
                var subject = "Your Order has been placed.";
                var body = $"Dear,{order.FirstName + " " + order.LastName} <br><br> We are excited for you to received your order #{order.Id}.";
                await _publisher.Publish(new OrderGeneratedNotification(to, subject, body));
            }
            return _mapper.Map<OrderResponse>(newOrder);
        }
    }
}
