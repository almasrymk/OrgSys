using Accounting.Application.Journals.Commands;
using Accounting.Domain.Repositories;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalCancelCommandHandlerTests
{
    private static (CancelJournalCommandHandler handler, Mock<IJournalRepository> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IJournalRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var integrationEventPublisher = new Mock<IIntegrationEventPublisher>();

        var handler = new CancelJournalCommandHandler(unitOfWork.Object, repository.Object, integrationEventPublisher.Object);

        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_DraftJournal_CancelsSuccessfully()
    {
        var journal = new Journal { Id = 1, Posted = false, Status = Status.New };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new CancelJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(Status.Cancel, journal.Status);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsCancel_MustUseReverseInstead()
    {
        var journal = new Journal { Id = 1, Posted = true, Status = Status.New };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new CancelJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Contains("Reverse", result.Errors!.Select(e => e.MessageError).First());
        Assert.Equal(Status.New, journal.Status);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
