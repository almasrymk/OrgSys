namespace Treasury.Application.CashBoxes.Validators
{
    using Treasury.Application.CashBoxes.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class CreateCashBoxCommandValidator : Validator<CreateCashBoxCommand, Treasury.Domain.CashBox>
    {
        public CreateCashBoxCommandValidator(IRepository<Treasury.Domain.CashBox> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The cash box name already exists")
            .OverridePropertyName(nameof(CreateCashBoxCommand.Name));
        }
    }
}
