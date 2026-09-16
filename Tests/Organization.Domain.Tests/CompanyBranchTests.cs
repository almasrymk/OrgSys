namespace Organization.Domain.Tests;

public class CompanyBranchTests
{
    [Fact]
    public void Branch_must_belong_to_a_company()
    {
        var company = new Company { Id = 3, LegalName = "OrgSys Co" };
        var branch = new Branch { Name = "Main", CompanyId = company.Id, Company = company };

        company.Branches.Add(branch);

        Assert.Equal(company.Id, branch.CompanyId);
        Assert.Contains(branch, company.Branches);
    }

    [Fact]
    public void Company_tenant_id_is_optional_in_stage_one()
    {
        var company = new Company { LegalName = "OrgSys Co" };

        Assert.Null(company.TenantId);
        Assert.Empty(company.Branches);
    }

    [Fact]
    public void Organization_settings_are_per_company_and_not_a_generic_store()
    {
        var settings = new OrganizationSettings
        {
            CompanyId = 3,
            FiscalYearStartMonth = 1,
            FiscalYearStartDay = 1,
            DefaultTimeZone = "Africa/Cairo"
        };

        Assert.Equal(3, settings.CompanyId);
        Assert.True(settings.FiscalYearStartMonth is >= 1 and <= 12);
        Assert.True(settings.FiscalYearStartDay is >= 1 and <= 31);
    }
}
