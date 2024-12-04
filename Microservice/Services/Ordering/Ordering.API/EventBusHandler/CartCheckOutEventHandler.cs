using AutoMapper;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;
using Shared.API.Events;

namespace Ordering.API.EventBusHandler
{
    public class CartCheckOutEventHandler : IConsumer<CartCheckOutEvent>
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly ILogger<CartCheckOutEventHandler> _logger;

        public CartCheckOutEventHandler(ISender sender, IMapper mapper, ILogger<CartCheckOutEventHandler> logger)
        {
            _sender = sender;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CartCheckOutEvent> context)
        {
            var order = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var newOrder = await _sender.Send(order);
            if (newOrder is not null)
            {
                _logger.LogInformation($"Cart checkout event completed!.Order username: {order.UserName}");
            }
            else
            {
                _logger.LogInformation($"Cart checkout event failed for {order.UserName}.");
            }
        }
    }
}
