namespace MasterData.Application.Currencies.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Currencies.Commands;

    public class UpdateCurrencyCommandValidator : Validator<UpdateCurrencyCommand, MasterData.Domain.Currency>
    {
        public UpdateCurrencyCommandValidator(IRepository<MasterData.Domain.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
                         
            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}
