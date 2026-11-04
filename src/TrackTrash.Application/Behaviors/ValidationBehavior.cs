using FluentValidation;
using MediatR;

namespace TrackTrash.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));
            var errors = results.SelectMany(x => x.Errors).Where(x => x is not null).ToArray();
            if (errors.Length != 0) throw new ValidationException(errors);
        }

        return await next();
    }
}
