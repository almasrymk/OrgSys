using Moq;
using OrgSys.Messaging;
using OrgSys.SharedKernel;
using Xunit;

namespace Application.Tests;

public class OutboxIntegrationEventPublisherTests
{
    [Fact]
    public async Task Publish_PersistsOutboxRowWithEventId()
    {
        var repository = new Mock<IRepository<OutboxMessage>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<OutboxMessage>())).ReturnsAsync((OutboxMessage m) => m);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var publisher = new OutboxIntegrationEventPublisher(repository.Object, unitOfWork.Object);
        var evt = new CommercialDocuments.Contracts.IntegrationEvents.SalesInvoicePostedIntegrationEvent(
            1, "INV", 2, DateTime.UtcNow, DateTime.UtcNow, 1, 1, 10, 1, DateTime.UtcNow, null);

        await publisher.PublishAsync(evt, CancellationToken.None);

        repository.Verify(r => r.CreateAsync(It.Is<OutboxMessage>(m =>
            m.EventId == evt.EventId && m.Payload.Contains("INV"))), Times.Once);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Inbox_SecondClaim_ReturnsFalse()
    {
        var existing = new InboxMessage { EventId = Guid.NewGuid(), HandlerName = "H" };
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(existing, 1);
        var repository = new Mock<IRepository<InboxMessage>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<InboxMessage, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);

        var store = new InboxStore(repository.Object, Mock.Of<IUnitOfWork>());
        Assert.False(await store.TryClaimAsync(existing.EventId, "H"));
        repository.Verify(r => r.CreateAsync(It.IsAny<InboxMessage>()), Times.Never);
    }
}
