namespace Application.Commands.Org.Setting.Currency.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Currency.Commands;
    using Utility;

    public class UpdateCurrencyCommandValidator : Validator<UpdateCurrencyCommand, Entity.Model.Currency>
    {
        public UpdateCurrencyCommandValidator(IRepository<Entity.Model.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Code , c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.Id != Ob.Id && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency code already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Code));

            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}