namespace Application.Commands.Org.Setting.Currency.Validators
{
    using Application.Commands.Org.Setting.Currency.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using System.Xml.Linq;
    using Utility;

    public class CreateCurrencyCommandValidator : Validator<CreateCurrencyCommand,  Entity.Model.Currency>
    {
        public CreateCurrencyCommandValidator(IRepository<Entity.Model.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Code })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency code already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Code));

            RuleFor(c => new { c.Name })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}