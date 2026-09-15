namespace Organization.Application.Companies.Validators
{
    using Organization.Application.Companies.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class UpdateCompanyCommandValidator : Validator<UpdateCompanyCommand, Organization.Domain.Company>
    {
        public UpdateCompanyCommandValidator(
            IRepository<Organization.Domain.Company> _Repository,
            IRepository<MasterData.Domain.Country> _CountryRepository,
            IRepository<MasterData.Domain.Currency> _CurrencyRepository) : base(_Repository)
        {
            RuleFor(c => c.LegalName)
            .NotEmpty().WithMessage("The legal name field is required");

            RuleFor(c => c.LegalName)
            .MaximumLength(150).WithMessage("The legal name must not exceed 150 characters");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.LegalName == command.LegalName && c.Id != command.Id, cancellationToken))
            .WithMessage("A company with this legal name already exists")
            .OverridePropertyName(nameof(UpdateCompanyCommand.LegalName));

            RuleFor(c => c.CountryId)
            .MustAsync(async (CountryId, cancellationToken) => CountryId == null || await _CountryRepository.AnyAsync(e => e.Id == CountryId, cancellationToken))
            .WithMessage("The country not found");

            RuleFor(c => c.DefaultCurrencyId)
            .MustAsync(async (CurrencyId, cancellationToken) => CurrencyId == null || await _CurrencyRepository.AnyAsync(e => e.Id == CurrencyId, cancellationToken))
            .WithMessage("The default currency not found");
        }
    }
}
