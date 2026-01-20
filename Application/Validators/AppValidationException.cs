using Domain.Shared;

namespace Application.Validators
{
    public class AppValidationException : Exception
    {
        public IReadOnlyList<Error> Errors { get; }

        public AppValidationException(List<Error> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }
    }
}