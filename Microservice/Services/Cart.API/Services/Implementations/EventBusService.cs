using Cart.API.Services.Interfaces;
using MassTransit;

namespace Cart.API.Services.Implementations
{
    public class EventBusService : IEventBusService
    {
        private readonly IPublishEndpoint _publishEndpoint;
     
        public EventBusService(IPublishEndpoint publishEndpoint) 
            => _publishEndpoint = publishEndpoint;
        
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
