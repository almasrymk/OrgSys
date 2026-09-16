using Advances.Application.Custodies.Commands;
using Advances.Domain;
using MediatR;
using Moq;
using OrgSys.SharedKernel;
using System.Net;
using Treasury.Contracts.Financials;
using Xunit;

namespace Application.Tests;

public class CustodyCommandHandlerTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);

    private static Custody NewDraft() => Custody.Create(1, "Travel", 1, 1, 1000, CreateDate.AddDays(10), 1, CreateDate);

    [Fact]
    public async Task Create_ValidCommand_PersistsCustody()
    {
        var repository = new Mock<IRepository<Custody>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Custody>())).ReturnsAsync((Custody c) => c);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCustodyCommandHandler(repository.Object, unitOfWork.Object);
        var result = await handler.Handle(new CreateCustodyCommand(1, "Travel", 1, 1, 1000, CreateDate.AddDays(10), 1, CreateDate, null, null, "C-1"), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<Custody>(c => c.Purpose == "Travel" && c.IssuedAmount == 1000)), Times.Once);
    }

    [Fact]
    public async Task Approve_DraftCustody_Succeeds()
    {
        var custody = NewDraft();
        var repository = new Mock<IRepository<Custody>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Custody, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(custody);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new ApproveCustodyCommandHandler(repository.Object, unitOfWork.Object);
        var result = await handler.Handle(new ApproveCustodyCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(CustodyStatus.Approved, custody.LifecycleStatus);
    }

    [Fact]
    public async Task Issue_ApprovedCustody_PostsTreasuryThenMarksIssued()
    {
        var custody = NewDraft();
        custody.Approve();
        var repository = new Mock<IRepository<Custody>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Custody, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(custody);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<PostCustodyFinancialCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<long>(HttpStatusCode.OK, 77, null));

        var handler = new IssueCustodyCommandHandler(repository.Object, unitOfWork.Object, sender.Object);
        var result = await handler.Handle(new IssueCustodyCommand(1, 10, 20, CreateDate, 1, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(CustodyStatus.Issued, custody.LifecycleStatus);
        Assert.Equal(77, custody.IssuingFinancialTransactionId);
    }

    [Fact]
    public async Task Issue_DraftCustody_IsRejectedWithoutTreasuryCall()
    {
        var custody = NewDraft();
        var repository = new Mock<IRepository<Custody>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Custody, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(custody);
        var unitOfWork = new Mock<IUnitOfWork>();
        var sender = new Mock<ISender>();

        var handler = new IssueCustodyCommandHandler(repository.Object, unitOfWork.Object, sender.Object);
        var result = await handler.Handle(new IssueCustodyCommand(1, 10, 20, CreateDate, 1, null, null), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        sender.Verify(s => s.Send(It.IsAny<PostCustodyFinancialCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
