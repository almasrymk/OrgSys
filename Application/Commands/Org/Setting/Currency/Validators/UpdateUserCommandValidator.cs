namespace Application.Commands.Org.Setting.Currency.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Currency.Commands;

    public class UpdateCurrencyCommandValidator : Validator<UpdateCurrencyCommand, Domain.Entities.Currency>
    {
        public UpdateCurrencyCommandValidator(IRepository<Domain.Entities.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
                         
            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}