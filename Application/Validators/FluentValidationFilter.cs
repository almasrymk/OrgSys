namespace Application.Validators
{
    using MediatR;
    using FluentValidation;
    using Application.Abstraction.Command;

    public class FluentValidationFilter<TRequest , TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : ICommand
        where TResponse : Domain.Shared.Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public FluentValidationFilter(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)))).SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                throw new AppValidationException(failures.Select(f => new Domain.Shared.Error(f.ErrorMessage , f.PropertyName)).ToList());
            }

            return await next();
        }
    }
}