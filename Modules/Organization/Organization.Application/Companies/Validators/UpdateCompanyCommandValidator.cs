namespace Organization.Application.Companies.Validators
{
    using Organization.Application.Companies.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;
    using MasterData.Contracts.Lookups;
    using MediatR;
    using SaaS.Contracts.Tenants;

    public class UpdateCompanyCommandValidator : Validator<UpdateCompanyCommand, Organization.Domain.Company>
    {
        public UpdateCompanyCommandValidator(
            IRepository<Organization.Domain.Company> _Repository,
            ISender sender) : base(_Repository)
        {
            RuleFor(c => c.TenantId)
            .MustAsync(async (TenantId, cancellationToken) =>
                TenantId == null || (await sender.Send(new TenantExistsQuery(TenantId.Value), cancellationToken)).Response)
            .WithMessage("The tenant not found");

            RuleFor(c => c.LegalName)
            .NotEmpty().WithMessage("The legal name field is required");

            RuleFor(c => c.LegalName)
            .MaximumLength(150).WithMessage("The legal name must not exceed 150 characters");

            RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await NotAnyAsync(c => c.LegalName == command.LegalName && c.Id != command.Id, cancellationToken))
            .WithMessage("A company with this legal name already exists")
            .OverridePropertyName(nameof(UpdateCompanyCommand.LegalName));

            RuleFor(c => c.CountryId)
            .MustAsync(async (CountryId, cancellationToken) =>
                CountryId == null || (await sender.Send(new ExistsCountryQuery(CountryId.Value), cancellationToken)).Response)
            .WithMessage("The country not found");

            RuleFor(c => c.DefaultCurrencyId)
            .MustAsync(async (CurrencyId, cancellationToken) =>
                CurrencyId == null || (await sender.Send(new ExistsCurrencyQuery(CurrencyId.Value), cancellationToken)).Response)
            .WithMessage("The default currency not found");
        }
    }
}
