using Accounting.Contracts.Accounts;
using Administration.Domain;
using Parties.Application.CustomerProfiles.Commands;
using Parties.Application.SupplierProfiles.Commands;
using Parties.Domain;
using OrgSys.SharedKernel;
using Moq;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

/// <summary>
/// Covers the Parties bounded context's central design decision (docs/parties/
/// party-target-architecture.md): a Dealer can gain the Customer role, the Supplier role, or both,
/// via AssignCustomerRoleCommand/AssignSupplierRoleCommand, without a duplicate identity row —
/// brief §2.9's mandatory "same real company is both customer and supplier" scenario, and the
/// PARTIES TEST EXAMPLES "Customer role cannot be assigned twice"/"Supplier role cannot be assigned
/// twice".
/// </summary>
public class PartyRoleAssignmentTests
{
    private static Dealer Client(long id = 1) => new() { Id = id, Name = "ABC Trading", TypeId = (long)DealerType.Client, Status = Status.New };

    private static Dealer Supplier(long id = 1) => new() { Id = id, Name = "ABC Trading", TypeId = (long)DealerType.Supplier, Status = Status.New };

    private static Mock<IRepository<Dealer>> DealerRepository(Dealer? dealer)
    {
        var repository = new Mock<IRepository<Dealer>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Dealer, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(dealer);
        return repository;
    }

    private static Mock<IReceivableAccountValidator> PassingAccountValidator() => AccountValidator(errors: []);

    private static Mock<IReceivableAccountValidator> AccountValidator(List<Error> errors)
    {
        var validator = new Mock<IReceivableAccountValidator>();
        validator.Setup(v => v.ValidateAccountAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new AccountLookupDto(1, "Acc", "AR-1", 1, "AR", true, true), errors));
        return validator;
    }

    private static Mock<IUnitOfWork> WorkingUnitOfWork()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return unitOfWork;
    }

    [Fact]
    public async Task AssignCustomerRole_DealerNotFound_ReturnsBadRequest()
    {
        var handler = new AssignCustomerRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(null).Object,
            new Mock<IRepository<CustomerProfile>>().Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignCustomerRoleCommand(1, 5, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("The party was not found.", result.Errors!.Select(e => e.MessageError));
    }

    [Fact]
    public async Task AssignCustomerRole_AlreadyAssigned_ReturnsBadRequestAndDoesNotCreateAnother()
    {
        var customerProfileRepository = new Mock<IRepository<CustomerProfile>>();
        customerProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CustomerProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new AssignCustomerRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Client()).Object,
            customerProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignCustomerRoleCommand(1, 5, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("The customer role is already assigned to this party.", result.Errors!.Select(e => e.MessageError));
        customerProfileRepository.Verify(r => r.CreateAsync(It.IsAny<CustomerProfile>()), Times.Never);
    }

    [Fact]
    public async Task AssignCustomerRole_ToASupplierDealer_Succeeds_DualRoleScenario()
    {
        // brief §2.9: a Dealer whose primary role (TypeId) is Supplier must still be able to gain
        // the Customer role too — the whole point of these commands existing.
        var customerProfileRepository = new Mock<IRepository<CustomerProfile>>();
        customerProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CustomerProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        CustomerProfile? created = null;
        customerProfileRepository.Setup(r => r.CreateAsync(It.IsAny<CustomerProfile>()))
            .Callback<CustomerProfile>(p => created = p)
            .ReturnsAsync((CustomerProfile p) => p);

        var handler = new AssignCustomerRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Supplier()).Object,
            customerProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignCustomerRoleCommand(1, 5, null, 10000m), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(1, created!.DealerId);
        Assert.Equal(5, created.AccountId);
        Assert.Equal(10000m, created.CreditLimit);
        Assert.True(created.IsCreditAllowed);
    }

    [Fact]
    public async Task AssignCustomerRole_InvalidExplicitAccount_ReturnsBadRequestAndDoesNotCreate()
    {
        var customerProfileRepository = new Mock<IRepository<CustomerProfile>>();
        customerProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CustomerProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new AssignCustomerRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Client()).Object,
            customerProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            AccountValidator([new Error("Account is not postable.")]).Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignCustomerRoleCommand(1, 5, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Account is not postable.", result.Errors!.Select(e => e.MessageError));
        customerProfileRepository.Verify(r => r.CreateAsync(It.IsAny<CustomerProfile>()), Times.Never);
    }

    [Fact]
    public async Task AssignCustomerRole_NoAccountAndAutoCreateNotRequested_ReturnsBadRequest()
    {
        var customerProfileRepository = new Mock<IRepository<CustomerProfile>>();
        customerProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<CustomerProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new AssignCustomerRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Client()).Object,
            customerProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignCustomerRoleCommand(1, null, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains(result.Errors!, e => e.MessageError.Contains("receivable account"));
        customerProfileRepository.Verify(r => r.CreateAsync(It.IsAny<CustomerProfile>()), Times.Never);
    }

    [Fact]
    public async Task AssignSupplierRole_ToAClientDealer_Succeeds_DualRoleScenario()
    {
        // Mirror of the Customer-side test — a Dealer whose primary role is Client must still be
        // able to gain the Supplier role too.
        var supplierProfileRepository = new Mock<IRepository<SupplierProfile>>();
        supplierProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<SupplierProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        SupplierProfile? created = null;
        supplierProfileRepository.Setup(r => r.CreateAsync(It.IsAny<SupplierProfile>()))
            .Callback<SupplierProfile>(p => created = p)
            .ReturnsAsync((SupplierProfile p) => p);

        var handler = new AssignSupplierRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Client()).Object,
            supplierProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignSupplierRoleCommand(1, 7, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(1, created!.DealerId);
        Assert.Equal(7, created.AccountId);
        Assert.True(created.IsApproved);
    }

    [Fact]
    public async Task AssignSupplierRole_AlreadyAssigned_ReturnsBadRequest()
    {
        var supplierProfileRepository = new Mock<IRepository<SupplierProfile>>();
        supplierProfileRepository.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<SupplierProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new AssignSupplierRoleCommandHandler(
            WorkingUnitOfWork().Object,
            DealerRepository(Supplier()).Object,
            supplierProfileRepository.Object,
            new Mock<IRepository<Preference>>().Object,
            PassingAccountValidator().Object,
            new Mock<MediatR.ISender>().Object);

        var result = await handler.Handle(new AssignSupplierRoleCommand(1, 7, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("The supplier role is already assigned to this party.", result.Errors!.Select(e => e.MessageError));
        supplierProfileRepository.Verify(r => r.CreateAsync(It.IsAny<SupplierProfile>()), Times.Never);
    }
}
