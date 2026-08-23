using Application.Commands.Org.Financials.Journal.Commands;
using AutoMapper;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalRedoCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<global::MappingProfile>());
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    private static (RedoJournalCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new RedoJournalCommandHandler(unitOfWork.Object, repository.Object, BuildMapper(), Mock.Of<IServiceProvider>());

        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_CancelledDraft_ReopensToNew()
    {
        var journal = new Journal { Id = 1, Posted = false, Status = Status.Cancel };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(Status.New, journal.Status);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsRedo_NeverReopened()
    {
        // A journal that somehow carries Posted=true — must never be reopened, regardless of Status.
        var journal = new Journal { Id = 1, Posted = true, Status = Status.Cancel };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal(Status.Cancel, journal.Status);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReversedJournal_RejectsRedo_AccountingHistoryStaysImmutable()
    {
        var journal = new Journal { Id = 1, Posted = true, Status = Status.Reversed };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal(Status.Reversed, journal.Status);
    }

    [Fact]
    public async Task Handle_JournalNotCancelled_NoOp()
    {
        var journal = new Journal { Id = 1, Posted = false, Status = Status.New };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new RedoJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, unitOfWork) = BuildHandler(null);

        var result = await handler.Handle(new RedoJournalCommand(999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
