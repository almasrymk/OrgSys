namespace Organization.Application.OrganizationSettings.Validators
{
    using Organization.Application.OrganizationSettings.Commands;
    using FluentValidation;

    public class UpdateOrganizationSettingsCommandValidator : AbstractValidator<UpdateOrganizationSettingsCommand>
    {
        public UpdateOrganizationSettingsCommandValidator(
            OrgSys.SharedKernel.IRepository<Organization.Domain.Company> _CompanyRepository,
            OrgSys.SharedKernel.IRepository<MasterData.Domain.Country> _CountryRepository,
            OrgSys.SharedKernel.IRepository<MasterData.Domain.Currency> _CurrencyRepository)
        {
            RuleFor(c => c.CompanyId)
            .GreaterThan(0).WithMessage("The company field is required");

            RuleFor(c => c.CompanyId)
            .MustAsync(async (CompanyId, cancellationToken) => await _CompanyRepository.AnyAsync(e => e.Id == CompanyId, cancellationToken))
            .WithMessage("The company not found");

            RuleFor(c => c.DefaultCountryId)
            .MustAsync(async (CountryId, cancellationToken) => CountryId == null || await _CountryRepository.AnyAsync(e => e.Id == CountryId, cancellationToken))
            .WithMessage("The default country not found");

            RuleFor(c => c.DefaultCurrencyId)
            .MustAsync(async (CurrencyId, cancellationToken) => CurrencyId == null || await _CurrencyRepository.AnyAsync(e => e.Id == CurrencyId, cancellationToken))
            .WithMessage("The default currency not found");
        }
    }
}
