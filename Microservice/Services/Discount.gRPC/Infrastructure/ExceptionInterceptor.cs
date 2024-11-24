using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Discount.gRPC.Infrastructure
{
    public class ExceptionInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;
        private readonly Guid _correlationId;
        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
            _correlationId = Guid.NewGuid();
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error occurred while processing your request: {exception.Message}", exception);
                throw exception.Handle(context, _logger, _correlationId);
            }
        }
    }
}
