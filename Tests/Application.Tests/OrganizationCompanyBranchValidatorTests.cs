using Organization.Application;
using Organization.Domain;
using Organization.Application.Branches.Commands;
using Organization.Application.Branches.Validators;
using Organization.Application.Companies.Commands;
using Organization.Application.Companies.Validators;
using Organization.Application.OrganizationSettings.Commands;
using OrgSys.SharedKernel;
using MasterData.Contracts.Lookups;
using MediatR;
using Moq;
using SaaS.Contracts.Tenants;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

/// <summary>
/// Covers the Organization/Administration bounded context's own invariants (brief's ORGANIZATION
/// TEST EXAMPLES: "Branch Code unique per Company" / "Cannot activate branch under inactive
/// company" — implemented here as Name-uniqueness, since Branch doesn't expose Code today, see
/// docs/organization/organization-target-architecture.md). FiscalYear/Currency invariants stay
/// covered wherever Accounting/MasterData already test them — they were not relocated into
/// Organization this pass.
/// </summary>
public class OrganizationCompanyBranchValidatorTests
{
    private static Mock<IRepository<Company>> CompanyRepository(params Company[] existing)
    {
        var repository = new Mock<IRepository<Company>>();
        repository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns<Expression<Func<Company, bool>>, CancellationToken>((expr, _) => new ValueTask<bool>(existing.AsQueryable().Any(expr)));
        return repository;
    }

    private static Mock<IRepository<Branch>> BranchRepository(params Branch[] existing)
    {
        var repository = new Mock<IRepository<Branch>>();
        repository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Branch, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns<Expression<Func<Branch, bool>>, CancellationToken>((expr, _) => new ValueTask<bool>(existing.AsQueryable().Any(expr)));
        return repository;
    }

    private static Mock<ISender> LookupSender(bool currencyExists = true, bool countryExists = true, bool tenantExists = true)
    {
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ExistsCurrencyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExistsCurrencyQuery query, CancellationToken _) =>
                new Result<bool>(HttpStatusCode.OK, currencyExists && query.Id != 999, null));
        sender.Setup(s => s.Send(It.IsAny<ExistsCountryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExistsCountryQuery query, CancellationToken _) =>
                new Result<bool>(HttpStatusCode.OK, countryExists, null));
        sender.Setup(s => s.Send(It.IsAny<TenantExistsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TenantExistsQuery query, CancellationToken _) =>
                new Result<bool>(HttpStatusCode.OK, tenantExists, null));
        return sender;
    }

    [Fact]
    public async Task CreateBranch_UnderActiveCompany_UniqueName_IsValid()
    {
        var activeCompany = new Company { Id = 1, LegalName = "ABC", Hide = false };
        var validator = new CreateBranchCommandValidator(BranchRepository().Object, CompanyRepository(activeCompany).Object);

        var result = await validator.ValidateAsync(new CreateBranchCommand("Cairo Branch", 1));

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreateBranch_MissingCompany_IsInvalid()
    {
        var validator = new CreateBranchCommandValidator(BranchRepository().Object, CompanyRepository().Object);

        var result = await validator.ValidateAsync(new CreateBranchCommand("Cairo Branch", 1));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "The company not found");
    }

    [Fact]
    public async Task CreateBranch_UnderInactiveCompany_IsInvalid()
    {
        var inactiveCompany = new Company { Id = 1, LegalName = "ABC", Hide = true };
        var validator = new CreateBranchCommandValidator(BranchRepository().Object, CompanyRepository(inactiveCompany).Object);

        var result = await validator.ValidateAsync(new CreateBranchCommand("Cairo Branch", 1));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "An inactive company cannot receive new branches");
    }

    [Fact]
    public async Task CreateBranch_DuplicateNameInSameCompany_IsInvalid()
    {
        var company = new Company { Id = 1, LegalName = "ABC", Hide = false };
        var existingBranch = new Branch { Id = 5, Name = "Cairo Branch", CompanyId = 1 };
        var validator = new CreateBranchCommandValidator(BranchRepository(existingBranch).Object, CompanyRepository(company).Object);

        var result = await validator.ValidateAsync(new CreateBranchCommand("Cairo Branch", 1));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "A branch with this name already exists in this company");
    }

    [Fact]
    public async Task CreateBranch_SameNameInDifferentCompany_IsValid()
    {
        var company = new Company { Id = 2, LegalName = "XYZ", Hide = false };
        // Same branch name exists, but under a different company — must not collide.
        var existingBranch = new Branch { Id = 5, Name = "Cairo Branch", CompanyId = 1 };
        var validator = new CreateBranchCommandValidator(BranchRepository(existingBranch).Object, CompanyRepository(company).Object);

        var result = await validator.ValidateAsync(new CreateBranchCommand("Cairo Branch", 2));

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task UpdateBranch_MovingIntoInactiveCompany_IsInvalid()
    {
        var inactiveCompany = new Company { Id = 2, LegalName = "XYZ", Hide = true };
        var existingBranch = new Branch { Id = 5, Name = "Cairo Branch", CompanyId = 1 };
        var validator = new UpdateBranchCommandValidator(BranchRepository(existingBranch).Object, CompanyRepository(inactiveCompany).Object);

        var result = await validator.ValidateAsync(new UpdateBranchCommand(5, "Cairo Branch", 2));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "An inactive company cannot receive branches");
    }

    [Fact]
    public async Task UpdateBranch_RenamingItself_DoesNotCollideWithItself()
    {
        var company = new Company { Id = 1, LegalName = "ABC", Hide = false };
        var existingBranch = new Branch { Id = 5, Name = "Cairo Branch", CompanyId = 1 };
        var validator = new UpdateBranchCommandValidator(BranchRepository(existingBranch).Object, CompanyRepository(company).Object);

        // Same Id, same Name+CompanyId as itself — must not trip the uniqueness check.
        var result = await validator.ValidateAsync(new UpdateBranchCommand(5, "Cairo Branch", 1));

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task CreateCompany_DuplicateLegalName_IsInvalid()
    {
        var existing = new Company { Id = 1, LegalName = "ABC" };
        var validator = new CreateCompanyCommandValidator(CompanyRepository(existing).Object, LookupSender().Object);

        var result = await validator.ValidateAsync(new CreateCompanyCommand { LegalName = "ABC" });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "A company with this legal name already exists");
    }

    [Fact]
    public async Task CreateCompany_UnknownDefaultCurrency_IsInvalid()
    {
        var validator = new CreateCompanyCommandValidator(CompanyRepository().Object, LookupSender(currencyExists: false).Object);

        var result = await validator.ValidateAsync(new CreateCompanyCommand { LegalName = "New Co", DefaultCurrencyId = 999 });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "The default currency not found");
    }

    [Fact]
    public async Task UpdateOrganizationSettings_NoExistingRow_CreatesOne()
    {
        var repository = new Mock<IRepository<Organization.Domain.OrganizationSettings>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Organization.Domain.OrganizationSettings, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Organization.Domain.OrganizationSettings?)null);

        Organization.Domain.OrganizationSettings? created = null;
        repository.Setup(r => r.CreateAsync(It.IsAny<Organization.Domain.OrganizationSettings>()))
            .Callback<Organization.Domain.OrganizationSettings>(s => created = s)
            .ReturnsAsync((Organization.Domain.OrganizationSettings s) => s);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateOrganizationSettingsCommandHandler(unitOfWork.Object, repository.Object);

        var result = await handler.Handle(new UpdateOrganizationSettingsCommand(1, 2, 3, "Africa/Cairo", 1, 1), CancellationToken.None);

        Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(1, created!.CompanyId);
        Assert.Equal(2, created.DefaultCurrencyId);
        repository.Verify(r => r.CreateAsync(It.IsAny<Organization.Domain.OrganizationSettings>()), Times.Once);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Organization.Domain.OrganizationSettings>()), Times.Never);
    }

    [Fact]
    public async Task UpdateOrganizationSettings_ExistingRow_UpdatesInPlace()
    {
        var existing = new Organization.Domain.OrganizationSettings { Id = 7, CompanyId = 1, DefaultTimeZone = "UTC" };
        var repository = new Mock<IRepository<Organization.Domain.OrganizationSettings>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Organization.Domain.OrganizationSettings, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Organization.Domain.OrganizationSettings>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateOrganizationSettingsCommandHandler(unitOfWork.Object, repository.Object);

        var result = await handler.Handle(new UpdateOrganizationSettingsCommand(1, null, null, "Africa/Cairo", 7, 1), CancellationToken.None);

        Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("Africa/Cairo", existing.DefaultTimeZone);
        Assert.Equal(7, existing.FiscalYearStartMonth);
        repository.Verify(r => r.CreateAsync(It.IsAny<Organization.Domain.OrganizationSettings>()), Times.Never);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Organization.Domain.OrganizationSettings>()), Times.Once);
    }
}
