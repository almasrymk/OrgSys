namespace Application.Commands.Org.Setting.CashBox.Validators
{
    using Application.Commands.Org.Setting.CashBox.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;

    public class CreateCashBoxCommandValidator : Validator<CreateCashBoxCommand, Domain.Entities.CashBox>
    {
        public CreateCashBoxCommandValidator(IRepository<Domain.Entities.CashBox> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The cash box name already exists")
            .OverridePropertyName(nameof(CreateCashBoxCommand.Name));
        }
    }
}
