namespace Application.Commands.Org.Setting.CashBox.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.CashBox.Commands;

    public class UpdateCashBoxCommandValidator : Validator<UpdateCashBoxCommand, Domain.Entities.CashBox>
    {
        public UpdateCashBoxCommandValidator(IRepository<Domain.Entities.CashBox> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The cash box name already exists")
            .OverridePropertyName(nameof(UpdateCashBoxCommand.Name));
        }
    }
}
