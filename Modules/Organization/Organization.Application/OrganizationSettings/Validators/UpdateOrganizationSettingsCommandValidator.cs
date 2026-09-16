namespace Organization.Application.OrganizationSettings.Validators
{
    using Organization.Application.OrganizationSettings.Commands;
    using FluentValidation;
    using MasterData.Contracts.Lookups;
    using MediatR;
    using OrgSys.SharedKernel;

    public class UpdateOrganizationSettingsCommandValidator : AbstractValidator<UpdateOrganizationSettingsCommand>
    {
        public UpdateOrganizationSettingsCommandValidator(
            IRepository<Organization.Domain.Company> _CompanyRepository,
            ISender sender)
        {
            RuleFor(c => c.CompanyId)
            .GreaterThan(0).WithMessage("The company field is required");

            RuleFor(c => c.CompanyId)
            .MustAsync(async (CompanyId, cancellationToken) => await _CompanyRepository.AnyAsync(e => e.Id == CompanyId, cancellationToken))
            .WithMessage("The company not found");

            RuleFor(c => c.DefaultCountryId)
            .MustAsync(async (CountryId, cancellationToken) =>
                CountryId == null || (await sender.Send(new ExistsCountryQuery(CountryId.Value), cancellationToken)).Response)
            .WithMessage("The default country not found");

            RuleFor(c => c.DefaultCurrencyId)
            .MustAsync(async (CurrencyId, cancellationToken) =>
                CurrencyId == null || (await sender.Send(new ExistsCurrencyQuery(CurrencyId.Value), cancellationToken)).Response)
            .WithMessage("The default currency not found");
        }
    }
}
