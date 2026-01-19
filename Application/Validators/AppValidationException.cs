namespace Application.Validators
{
    public class AppValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public AppValidationException(List<string> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }
    }
}