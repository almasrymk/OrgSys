using Accounting.Application.Journals.Commands;
using Accounting.Domain.Repositories;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalRedoCommandHandlerTests
{
    private static Journal NewJournal(long id, bool posted, Status status)
    {
        var journal = Journal.CreateDraft(1, 1, 1, "GJ-1", DateTime.Today, 1, DateTime.Today, null, null, 1, 1, null);
        journal.Id = id;
        journal.Posted = posted;
        journal.Status = status;
        return journal;
    }

    private static (RedoJournalCommandHandler handler, Mock<IJournalRepository> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IJournalRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new RedoJournalCommandHandler(unitOfWork.Object, repository.Object);

        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_CancelledDraft_ReopensToNew()
    {
        var journal = NewJournal(1, posted: false, status: Status.Cancel);
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(Status.New, journal.Status);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsRedo_NeverReopened()
    {
        // A journal that somehow carries Posted=true — must never be reopened, regardless of Status.
        var journal = NewJournal(1, posted: true, status: Status.Cancel);
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal(Status.Cancel, journal.Status);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReversedJournal_RejectsRedo_AccountingHistoryStaysImmutable()
    {
        var journal = NewJournal(1, posted: true, status: Status.Reversed);
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal(Status.Reversed, journal.Status);
    }

    [Fact]
    public async Task Handle_JournalNotCancelled_NoOp()
    {
        var journal = NewJournal(1, posted: false, status: Status.New);
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, unitOfWork) = BuildHandler(null);

        var result = await handler.Handle(new RedoJournalCommand(999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
