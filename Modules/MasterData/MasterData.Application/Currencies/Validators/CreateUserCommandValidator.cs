namespace MasterData.Application.Currencies.Validators
{
    using MasterData.Application.Currencies.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;
    using System.Xml.Linq;

    public class CreateCurrencyCommandValidator : Validator<CreateCurrencyCommand,  MasterData.Domain.Currency>
    {
        public CreateCurrencyCommandValidator(IRepository<MasterData.Domain.Currency> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The currency name already exists")
            .OverridePropertyName(nameof(CreateCurrencyCommand.Name));
        }
    }
}
