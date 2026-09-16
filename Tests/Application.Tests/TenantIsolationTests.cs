using AutoMapper;
using Moq;
using Organization.Application;
using Organization.Application.Companies.Commands;
using Organization.Application.Companies.Queries;
using Organization.Domain;
using OrgSys.SharedKernel;
using SaaS.Contracts.Features;
using SaaS.Contracts.Tenancy;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

public class TenantIsolationTests
{
    [Fact]
    public async Task CreateCompany_LimitReached_ReturnsForbidden()
    {
        var repository = new Mock<IRepository<Company>>();
        repository.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<Company, bool>>>()))
            .ReturnsAsync([new Company { LegalName = "Existing" }]);
        var current = new Mock<ICurrentTenant>();
        current.SetupGet(c => c.TenantId).Returns(10);
        var features = new Mock<ITenantFeatureService>();
        features.Setup(f => f.IsWithinLimitAsync(10, TenantLimit.Companies, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateCommandHandler(
            Mock.Of<IUnitOfWork>(),
            repository.Object,
            Mock.Of<IMapper>(),
            current.Object,
            features.Object);

        var result = await handler.Handle(new CreateCompanyCommand { LegalName = "New Co" }, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task GetCompany_TenantB_AsTenantA_ReturnsNotFound()
    {
        var company = new Company { LegalName = "B Co", TenantId = 20 };
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(company, 5);
        var repository = new Mock<IRepository<Company>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(company);
        var mapper = new Mock<IMapper>();
        mapper.Setup(m => m.Map<CompanyDto>(It.IsAny<Company>()))
            .Returns(new CompanyDto { Id = 5, TenantId = 20, LegalName = "B Co" });
        var current = new Mock<ICurrentTenant>();
        current.SetupGet(c => c.TenantId).Returns(10);

        var handler = new GetByIdQueryHandler(repository.Object, mapper.Object, current.Object);
        var result = await handler.Handle(new GetByIdCompanyQuery(5), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task UpdateCompany_TenantB_AsTenantA_ReturnsNotFound()
    {
        var company = new Company { LegalName = "B Co", TenantId = 20 };
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(company, 5);
        var repository = new Mock<IRepository<Company>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(company);
        var current = new Mock<ICurrentTenant>();
        current.SetupGet(c => c.TenantId).Returns(10);

        var handler = new UpdateCommandHandler(
            Mock.Of<IUnitOfWork>(),
            repository.Object,
            Mock.Of<IMapper>(),
            Mock.Of<IServiceProvider>(),
            current.Object);

        var result = await handler.Handle(new UpdateCompanyCommand { Id = 5, LegalName = "Hijack" }, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCompany_OtherTenant_FilterRequiresMatchingTenantId()
    {
        var current = new Mock<ICurrentTenant>();
        current.SetupGet(c => c.TenantId).Returns(10);
        var handler = new DeleteCommandHandler(
            Mock.Of<IUnitOfWork>(),
            Mock.Of<IRepository<Company>>(),
            Mock.Of<IServiceProvider>(),
            current.Object);

        var filter = handler.CreateFilter(new DeleteCompanyCommand(5)).Compile();
        Assert.False(filter(new Company { LegalName = "B", TenantId = 20, Status = Status.New }));
        var owned = new Company { LegalName = "A", TenantId = 10, Status = Status.New };
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(owned, 5);
        Assert.True(filter(owned));
    }
}
