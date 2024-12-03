﻿using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) 
            => _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
        
                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

                var failures = validationResults.Where(r => r.Errors.Count > 0)
                                                .SelectMany(r => r.Errors)
                                                .ToList();

                if (failures.Count > 0)
                {
                    throw new FluentValidation.ValidationException(failures);
                }

            }
            return await next().ConfigureAwait(false);
        }
    }
}