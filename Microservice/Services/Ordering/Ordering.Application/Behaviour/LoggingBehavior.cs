using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviour
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Handling {typeof(TRequest).Name}");
                var response = await next();
                _logger.LogInformation($"Handled {typeof(TResponse).Name}");
                return response;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, $"Unhandled Exception Occurred with Request Name: {requestName}, {request}");
                throw;
            }
        }
    }
}
