namespace Application.Commands.Org.Setting.Currency.Validators
{
    using Application.Commands.Org.Setting.Currency.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using System.Xml.Linq;
    using Utility;

    public class CreateCurrencyCommandValidator : Validator<CreateCurrencyCommand,  Domain.Entities.Currency>
    {
        public CreateCurrencyCommandValidator(IRepository<Domain.Entities.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}