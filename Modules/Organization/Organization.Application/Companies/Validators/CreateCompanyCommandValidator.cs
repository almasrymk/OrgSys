namespace Organization.Application.Companies.Validators
{
    using Organization.Application.Companies.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    /// <summary>
    /// CountryId/DefaultCurrencyId existence checks read MasterData.Domain directly via
    /// IRepository&lt;T&gt; — Company.CountryId/DefaultCurrencyId stay scalar-only (no Domain
    /// navigation, unlike Bank/Dealer), so this is an Application-layer-only cross-module read,
    /// the same accepted-exception shape already used elsewhere (see
    /// Tests/Architecture.Tests/ModuleLayerDependencyTests.cs AcceptedApplicationDomainExceptions
    /// — ("Organization", "MasterData", ...)).
    /// </summary>
    public class CreateCompanyCommandValidator : Validator<CreateCompanyCommand, Organization.Domain.Company>
    {
        public CreateCompanyCommandValidator(
            IRepository<Organization.Domain.Company> _Repository,
            IRepository<MasterData.Domain.Country> _CountryRepository,
            IRepository<MasterData.Domain.Currency> _CurrencyRepository) : base(_Repository)
        {
            RuleFor(c => c.LegalName)
            .NotEmpty().WithMessage("The legal name field is required");

            RuleFor(c => c.LegalName)
            .MaximumLength(150).WithMessage("The legal name must not exceed 150 characters");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.LegalName == command.LegalName, cancellationToken))
            .WithMessage("A company with this legal name already exists")
            .OverridePropertyName(nameof(CreateCompanyCommand.LegalName));

            RuleFor(c => c.CountryId)
            .MustAsync(async (CountryId, cancellationToken) => CountryId == null || await _CountryRepository.AnyAsync(e => e.Id == CountryId, cancellationToken))
            .WithMessage("The country not found");

            RuleFor(c => c.DefaultCurrencyId)
            .MustAsync(async (CurrencyId, cancellationToken) => CurrencyId == null || await _CurrencyRepository.AnyAsync(e => e.Id == CurrencyId, cancellationToken))
            .WithMessage("The default currency not found");
        }
    }
}
