﻿using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Shared.API.Response;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Cart.API.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
            => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"An error occurred while processing your request: {exception.Message}", exception);

            ErrorResponse errorResponse = exception switch
            {
                RpcException rpcEx when rpcEx.StatusCode == StatusCode.PermissionDenied => new(statusCode: (int)HttpStatusCode.Forbidden, title: rpcEx.GetType().Name, message: "Bad gRPC response"),
                ValidationException => new(statusCode: (int)HttpStatusCode.BadRequest, title: exception.GetType().Name, message: exception.Message),
                ArgumentNullException => new(statusCode: (int)HttpStatusCode.BadRequest, title: exception.GetType().Name, message: exception.Message),
                BadHttpRequestException => new(statusCode: (int)HttpStatusCode.BadRequest, title: exception.GetType().Name, message: exception.Message),
                FileNotFoundException => new(statusCode: (int)HttpStatusCode.NotFound, title: exception.GetType().Name, message: exception.Message),
                _ => new(statusCode: (int)HttpStatusCode.InternalServerError, title: "Internal Server Error", message: exception.Message)
            };

            httpContext.Response.StatusCode = errorResponse.StatusCode;
            await httpContext
                .Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);
            return true;
        }
    }
}
