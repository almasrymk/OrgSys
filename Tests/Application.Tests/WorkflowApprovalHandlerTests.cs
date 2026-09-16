using Moq;
using OrgSys.SharedKernel;
using Purchasing.Contracts.PurchaseRequisitions;
using System.Linq.Expressions;
using System.Net;
using Workflow.Application.Approvals.Commands;
using Workflow.Contracts.Approvals;
using Workflow.Domain;
using Xunit;

namespace Application.Tests;

public class WorkflowApprovalHandlerTests
{
    [Fact]
    public async Task Start_NewDocument_PersistsPendingRequest()
    {
        var repository = new Mock<IRepository<ApprovalRequest>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<ApprovalRequest, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((ApprovalRequest?)null);
        repository.Setup(r => r.CreateAsync(It.IsAny<ApprovalRequest>())).ReturnsAsync((ApprovalRequest a) => a);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new StartApprovalCommandHandler(unitOfWork.Object, repository.Object);
        var result = await handler.Handle(
            new StartApprovalCommand(WorkflowDocumentTypeCode.PurchaseRequisition, 15, 1, DateTime.UtcNow),
            CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<ApprovalRequest>(a => a.DocumentId == 15 && a.LifecycleStatus == ApprovalStatus.Pending)), Times.Once);
    }

    [Fact]
    public async Task Decide_Reject_NotifiesPurchasingWithoutTouchingPurchaseOrder()
    {
        var approval = ApprovalRequest.Start(WorkflowDocumentType.PurchaseRequisition, 15, 1, DateTime.UtcNow);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(approval, 9);

        var repository = new Mock<IRepository<ApprovalRequest>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<ApprovalRequest, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(approval);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var sender = new Mock<MediatR.ISender>();
        sender.Setup(s => s.Send(It.IsAny<ApplyPurchaseRequisitionWorkflowDecisionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result(HttpStatusCode.OK, null));

        var handler = new DecideApprovalCommandHandler(unitOfWork.Object, repository.Object, sender.Object);
        var result = await handler.Handle(
            new DecideApprovalCommand(9, 4, false, "no", DateTime.UtcNow),
            CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(ApprovalStatus.Rejected, approval.LifecycleStatus);
        sender.Verify(s => s.Send(It.Is<ApplyPurchaseRequisitionWorkflowDecisionCommand>(c => c.PurchaseRequisitionId == 15 && c.Approved == false), It.IsAny<CancellationToken>()), Times.Once);
    }
}
