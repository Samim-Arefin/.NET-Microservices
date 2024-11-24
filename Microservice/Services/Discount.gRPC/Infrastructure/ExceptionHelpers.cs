using Grpc.Core;

namespace Discount.gRPC.Infrastructure
{
    public static class ExceptionHelpers
    {
        public static RpcException Handle<T>(this Exception exception, ServerCallContext context, ILogger<T> logger, Guid correlationId) =>
        exception switch
        {
            TimeoutException => HandleTimeoutException((TimeoutException)exception, context, logger, correlationId),
            RpcException => HandleRpcException((RpcException)exception, logger, correlationId),
            _ => HandleDefault(exception, context, logger, correlationId)
        };

        private static RpcException HandleTimeoutException<T>(TimeoutException exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
        {
            logger.LogError(exception, $"CorrelationId: {correlationId} - A timeout occurred");
            var status = new Status(StatusCode.Internal, "An external resource did not answer within the time limit");
            return new RpcException(status, CreateTrailers(correlationId));
        }

        private static RpcException HandleRpcException<T>(RpcException exception, ILogger<T> logger, Guid correlationId)
        {
            logger.LogError(exception, $"CorrelationId: {correlationId} - An error occurred");
            var status = new Status(exception.StatusCode, exception.Message);
            return new RpcException(status, CreateTrailers(correlationId, exception.Trailers));
        }

        private static RpcException HandleDefault<T>(Exception exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
        {
            logger.LogError(exception, $"CorrelationId: {correlationId} - An error occurred");
            var status = new Status(StatusCode.Internal, exception.Message);
            return new RpcException(status, CreateTrailers(correlationId));
        }

        private static Metadata CreateTrailers(Guid correlationId, Metadata? trailersToAdd = null)
        {
            var trailers = new Metadata
            {
               {
                 "CorrelationId", correlationId.ToString() 
               }
            };

            if (trailersToAdd is null)
            {
                return trailers;
            }

            foreach (var trailer in trailersToAdd)
            {
                trailers.Add(trailer);
            }

            return trailers;
        }
    }
}
